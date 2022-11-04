using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using OutlayManager.ExternalService.Model;
using OutlayManager.ExternalService.TransactionBus.Abstract;
using OutlayManager.ExternalService.TransactionBus.Resources;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace OutlayManager.ExternalService.TransactionBus.Implementation
{
    internal class AzureServiceBus : ITransactionServiceBus
    {
        private readonly Dictionary<string, string> configurationCache = new Dictionary<string, string>();

        private readonly ITransactionServices _transactionServices;
        private readonly IConfiguration _configuration;

        private AzureServiceBus() { }

        public AzureServiceBus(ITransactionServices transactionService, IConfiguration configuration)
        {   
            _transactionServices = transactionService ?? throw new ArgumentNullException(nameof(ITransactionServices));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(IConfiguration));
        }

        public async Task<IEnumerable<TransactionDTO>> SyncExternalTransactionAsync()
        {
            ServiceBusReceiver receiver = null;
            List<TransactionDTO> transactionsSaved = new List<TransactionDTO>();

            try
            {
                ServiceBusClient serviceBusClient = new ServiceBusClient(LoadConfigValue(AppConfigHelper.CONNECTION_KEY));
                string queueName = LoadConfigValue(AppConfigHelper.QUEUE_KEY);
                int maxMessages = LoadConfigValueAsInt(AppConfigHelper.MAX_MESSAGES_KEY);
                int messageTimeout = LoadConfigValueAsInt(AppConfigHelper.MESSAGE_TIMEOUT_KEY);

                receiver = serviceBusClient.CreateReceiver(queueName, new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.PeekLock });

                TimeSpan messagesTimeOut = new TimeSpan(0, 0, messageTimeout);

                IReadOnlyList<ServiceBusReceivedMessage> messagesReceived = await receiver.ReceiveMessagesAsync(maxMessages, messagesTimeOut);

                foreach (var messageAux in messagesReceived)
                {
                    TransactionDTO transaction = MessageToTransactionModel(messageAux);

                    uint transactionId = await _transactionServices.SaveTransaction(transaction);
                    TransactionDTO transactionSaved = await _transactionServices.Transaction(transactionId);
                    transactionsSaved.Add(transactionSaved);

                    await receiver.CompleteMessageAsync(messageAux);
                }
            }
            finally
            {
                receiver?.DisposeAsync();
            }

            return transactionsSaved;
        }

        private TransactionDTO MessageToTransactionModel(ServiceBusReceivedMessage message)
        {
            TransactionMessageServiceBus transactionMessageAux = message.Body.ToObjectFromJson<TransactionMessageServiceBus>();

            TransactionDTO transactionDTO = new TransactionDTO()
            {
                Amount = transactionMessageAux.Amount,
                Date = transactionMessageAux.Date,
                Description = transactionMessageAux.Description,
                CodeTransactionID = transactionMessageAux.CodeID,
                TypeTransactionID = transactionMessageAux.TypeID
            };

            return transactionDTO;
        }

        private string LoadConfigValue(string key)
        {
            if (!configurationCache.TryGetValue(key, out string value))
            {
                value = _configuration.GetSection(key)?.Value ?? throw new Exception($"Configuration {key} is not defined!");
                configurationCache.Add(key, value);
            }

            return value;
        }

        private int LoadConfigValueAsInt(string key)
        {
            string value = LoadConfigValue(key);

            return Int32.Parse(value, NumberFormatInfo.InvariantInfo);
        }
    }
}
