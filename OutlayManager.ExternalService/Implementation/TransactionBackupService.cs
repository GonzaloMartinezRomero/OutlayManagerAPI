using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using OutlayManager.ExternalService.Abstract;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace OutlayManager.ExternalService.Implementation
{
    internal sealed class TransactionBackupService : ITransactionBackup
    {
        private const string BLOB_BACKUP_NAME_KEY = "BlobName";
        private const string AZURE_BLOB_CONNECTION_STRING_KEY = "AzureBlobConnectionString";
        private const string AZURE_CONTAINER_KEY = "AzureContainerName";

        private readonly string connectionStringBlobStorage;
        private readonly string azureContainerName;
        private readonly string blobBackupName;

        private readonly BlobServiceClient _blobService;
        private readonly ITransactionServices _transactionService;

        public TransactionBackupService(IConfiguration configuration, ITransactionServices transactionServices)
        {
            connectionStringBlobStorage = configuration.GetSection(AZURE_BLOB_CONNECTION_STRING_KEY)?.Value ?? throw new Exception($"Configuration {AZURE_BLOB_CONNECTION_STRING_KEY} not defined!");
            azureContainerName = configuration.GetSection(AZURE_CONTAINER_KEY)?.Value ?? throw new Exception($"Configuration {AZURE_CONTAINER_KEY} not defined!");
            blobBackupName = configuration.GetSection(BLOB_BACKUP_NAME_KEY)?.Value ?? throw new Exception($"Configuration {BLOB_BACKUP_NAME_KEY} not defined!");

            _blobService = new BlobServiceClient(connectionStringBlobStorage);
            _transactionService = transactionServices ?? throw new ArgumentNullException($"{nameof(transactionServices)} is null");
        }

        public async Task BackupDataBaseAsync()
        {
            Stream streamContent = null;

            try
            {
                IEnumerable<TransactionDTO> transactions = await _transactionService.Transactions();

                streamContent = GenerateJsonStream(transactions);

                var container = _blobService.GetBlobContainerClient(azureContainerName);
                var blobClient = container.GetBlobClient(blobBackupName);

                await blobClient.UploadAsync(streamContent, overwrite: true);                
            }
            finally
            {
                if(streamContent != null)
                    await streamContent.DisposeAsync();
            }
        }

        private Stream GenerateJsonStream(IEnumerable<TransactionDTO> transactions)
        {
            string jsonTransaction = JsonSerializer.Serialize(transactions, typeof(IEnumerable<TransactionDTO>));

            Stream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(jsonTransaction);
            streamWriter.Flush();

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
