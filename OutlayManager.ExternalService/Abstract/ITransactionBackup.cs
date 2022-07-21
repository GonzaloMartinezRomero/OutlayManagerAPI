using System.Threading.Tasks;

namespace OutlayManager.ExternalService.Abstract
{
    public interface ITransactionBackup
    {
        /// <summary>
        /// Create a backup of data base in json format
        /// </summary>
        /// <returns></returns>
        Task BackupDataBaseAsync();

        /// <summary>
        /// Download latest backup
        /// </summary>
        /// <returns></returns>
        Task<byte[]> DownloadBackupFileAsync();
    }
}
