using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using OutlayManager.Infraestructure.Services.Abstract;
using OutlayManager.Model.DTO;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace OutlayManager.Infraestructure.Services.Implementation
{
    internal class AzureTransactionQueue : ITransactionPending
    {
        private const string QUEUE_CONNECTION_STRING = "AzureTransactionQueue:ConnectionString";
        private const string QUEUE_NAME = "AzureTransactionQueue:Name";
        private const int MAX_MESSAGES_RECEIVED = 30;

        private readonly QueueClient queueClient;
        private readonly ITransactionServices _transactionService;

        private QueueClientOptions TransactionMessageOptions
        {
            get
            {
                return new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 };
            }
        }
        public AzureTransactionQueue(IConfiguration configuration, ITransactionServices transactionService)
        {
            _transactionService = transactionService ?? throw new ArgumentException(nameof(ITransactionServices));

            string connectionString = configuration[QUEUE_CONNECTION_STRING] ?? throw new ArgumentException(nameof(QUEUE_CONNECTION_STRING));
            string queueName = configuration[QUEUE_NAME] ?? throw new ArgumentException(nameof(QUEUE_NAME));

            queueClient = new QueueClient(connectionString, queueName, TransactionMessageOptions);
        }

        public IEnumerable<TransactionDTO> SavePendingTransactions()
        {
            List<TransactionDTO> transactionQueueds = new List<TransactionDTO>(); 

            var messages = queueClient.ReceiveMessages(MAX_MESSAGES_RECEIVED, TimeSpan.FromSeconds(1));

            foreach (var message in messages.Value)
            {
                string transactionJsonMessage = message.Body.ToString();
                TransactionQueued transactionQueued = JsonSerializer.Deserialize<TransactionQueued>(transactionJsonMessage);

                TransactionDTO transactionDto = ToTransactionDto(transactionQueued);

                _transactionService.SaveTransaction(transactionDto);

                transactionQueueds.Add(transactionDto);
            }

            ClearPendingTransactions();

            return transactionQueueds;
        }

        private static TransactionDTO ToTransactionDto(TransactionQueued transactionQueued)
        {
            return new TransactionDTO()
            {
                Amount = transactionQueued.Amount,
                Date = transactionQueued.Date,
                Description = transactionQueued.Description,
                CodeTransactionID = transactionQueued.CodeID,
                TypeTransactionID = transactionQueued.TypeID
            };
        }

        private void ClearPendingTransactions() => queueClient.ClearMessages();
    }
}
