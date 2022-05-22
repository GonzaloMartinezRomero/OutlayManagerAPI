using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using OutlayManager.Infraestructure.Model;
using OutlayManager.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlayManager.Infraestructure.Services.Implementation
{
    internal class ExternalTransactionService : IExternalTransactionService
    {
        private const string CONNECTION_KEY = "StringConnectionServiceBus";
        private const string QUEUE_KEY = "QueueServiceBus";
        private const string MAX_MESSAGES_KEY = "MaxMessagesServiceBus";

        private readonly string CONNECTION_SERVICE_BUS;
        private readonly string QUEUE_NAME;
        private readonly int MAX_MESSAGES;

        private readonly ServiceBusClient _serviceBusClient;
        private readonly ITransactionServices _transactionServices;

        private ExternalTransactionService() { }

        public ExternalTransactionService(ITransactionServices transactionService, IConfiguration configuration)
        {
            CONNECTION_SERVICE_BUS = configuration.GetSection(CONNECTION_KEY)?.Value ?? throw new Exception($"Configuration {CONNECTION_KEY} not defined!");
            QUEUE_NAME = configuration.GetSection(QUEUE_KEY)?.Value ?? throw new Exception($"Configuration {QUEUE_KEY} not defined!");

            string maxMessagesValue = configuration.GetSection(MAX_MESSAGES_KEY)?.Value ?? throw new Exception($"Configuration {MAX_MESSAGES_KEY} not defined!");
            if (!Int32.TryParse(maxMessagesValue, out MAX_MESSAGES))
                throw new Exception($"Value for{MAX_MESSAGES_KEY} must be Int32");

            _serviceBusClient = new ServiceBusClient(CONNECTION_SERVICE_BUS);
            _transactionServices = transactionService ?? throw new ArgumentNullException($"{nameof(transactionService)} is null");
        }

        public async Task<IEnumerable<TransactionDTO>> DownloadExternalTransaction()
        {
            ServiceBusReceiver receiver = null;
            List<TransactionDTO> transactionsSaved = new List<TransactionDTO>();

            try
            {
                receiver = _serviceBusClient.CreateReceiver(QUEUE_NAME, new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.PeekLock });

                IReadOnlyList<ServiceBusReceivedMessage> messagesReceived = await receiver.ReceiveMessagesAsync(MAX_MESSAGES);

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

        public void Dispose()
        {
            _serviceBusClient?.DisposeAsync();
        }
    }
}
