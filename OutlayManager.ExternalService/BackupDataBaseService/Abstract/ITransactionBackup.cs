using System.Threading.Tasks;

namespace OutlayManager.ExternalService.BackupDataBaseService.Abstract
{
    public interface ITransactionBackup
    {
        /// <summary>
        /// Create a backup of data base transactions 
        /// </summary>
        /// <returns></returns>
        Task BackupTransactionsAsync();

        /// <summary>
        /// Download latest backup
        /// </summary>
        /// <returns></returns>
        Task<byte[]> DownloadBackupFileAsync();
    }
}
