using System.IO;

namespace OutlayManager.ExternalService.BackupDataBaseService.Resources
{
    internal static class AppConfigHelper
    {
        public const string BLOB_BACKUP_NAME = "JsonBlobName";
        public const string BLOB_DB_NAME = "DbBlobName";
        public const string AZURE_BLOB_CONNECTION_STRING_KEY = "AzureBlobConnectionString";
        public const string AZURE_JSON_CONTAINER_KEY = "AzureJsonContainer";
        public const string AZURE_DB_CONTAINER_KEY = "AzureDbContainer";
        public const string DB_CONNECTION_STRING = "ConnectionStrings:PathBDConnection";
        public const string DB_PATH = "PathBD";
    }
}
