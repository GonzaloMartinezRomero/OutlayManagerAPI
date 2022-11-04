using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using OutlayManager.ExternalService.BackupDataBaseService.Abstract;
using OutlayManager.ExternalService.BackupDataBaseService.Resources;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OutlayManager.ExternalService.BackupDataBaseService.Implementation
{
    internal sealed class TransactionBackupService : ITransactionBackup
    {
        private readonly Dictionary<string, string> configurationCache = new Dictionary<string, string>();
                
        private readonly ITransactionServices _transactionService;
        private readonly IConfiguration _configuration; 
        
        public TransactionBackupService(IConfiguration configuration, ITransactionServices transactionServices)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(IConfiguration));
            _transactionService = transactionServices ?? throw new ArgumentNullException(nameof(ITransactionServices));
        }

        public async Task BackupTransactionsAsync()
        {
            await BackupJsonFile();
            await BackupDataBaseFile();

            //await Task.WhenAll(new Task[] { backupJsonTask, backupDataBaseTask });
        }

        /// <inheritdoc/>
        public async Task<byte[]> DownloadBackupFileAsync()
        {
            BlobServiceClient _blobService = new BlobServiceClient(LoadConfigValue(AppConfigHelper.AZURE_BLOB_CONNECTION_STRING_KEY));

            var container = _blobService.GetBlobContainerClient(LoadConfigValue(AppConfigHelper.AZURE_JSON_CONTAINER_KEY));
            var blobsClient = container.GetBlobClient(LoadConfigValue(AppConfigHelper.BLOB_BACKUP_NAME));

            using Stream blobMemoryStream = new MemoryStream();

            await blobsClient.DownloadToAsync(blobMemoryStream);

            blobMemoryStream.Position = 0;
            using StreamReader reader = new StreamReader(blobMemoryStream);

            string blobContent = await reader.ReadToEndAsync();

            byte[] fileBytes = Encoding.ASCII.GetBytes(blobContent);

            return fileBytes;
        }

        private static Stream GenerateJsonStream(IEnumerable<TransactionDTO> transactions)
        {
            string jsonTransaction = JsonSerializer.Serialize(transactions, typeof(IEnumerable<TransactionDTO>));

            Stream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(jsonTransaction);
            streamWriter.Flush();

            memoryStream.Position = 0;

            return memoryStream;
        }

        private async Task BackupJsonFile()
        {
            Stream streamContent = null;

            try
            {
                IEnumerable<TransactionDTO> transactions = await _transactionService.Transactions();

                streamContent = GenerateJsonStream(transactions);

                BlobServiceClient _blobService = new BlobServiceClient(LoadConfigValue(AppConfigHelper.AZURE_BLOB_CONNECTION_STRING_KEY));

                var container = _blobService.GetBlobContainerClient(LoadConfigValue(AppConfigHelper.AZURE_JSON_CONTAINER_KEY));
                var blobClient = container.GetBlobClient(LoadConfigValue(AppConfigHelper.BLOB_BACKUP_NAME));

                await blobClient.UploadAsync(streamContent, overwrite: true);
            }
            finally
            {
                if (streamContent != null)
                    await streamContent.DisposeAsync();
            }
        }

        private async Task BackupDataBaseFile()
        {
            string pathDBFile = LoadConfigValue(AppConfigHelper.DB_PATH);            
            string blobContainerName = LoadConfigValue(AppConfigHelper.AZURE_DB_CONTAINER_KEY);
            string connectionString = LoadConfigValue(AppConfigHelper.AZURE_BLOB_CONNECTION_STRING_KEY);

            BlobServiceClient _blobService = new BlobServiceClient(connectionString);            

            BlobContainerClient containerClient = _blobService.GetBlobContainerClient(blobContainerName);
            var blobClient = containerClient.GetBlobClient(LoadConfigValue(AppConfigHelper.BLOB_DB_NAME));

            await blobClient.UploadAsync(pathDBFile, overwrite: true);
        }

        private string LoadConfigValue(string key)
        {
            if(!configurationCache.TryGetValue(key,out string value))
            {
                value = _configuration.GetSection(key)?.Value ?? throw new Exception($"Configuration {key} is not defined!");
                configurationCache.Add(key, value);
            }   

            return value;
        }
    }
}
