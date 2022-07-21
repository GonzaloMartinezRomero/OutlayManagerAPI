using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OutlayManager.ExternalService.Abstract;
using OutlayManager.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Controllers
{
    /// <summary>
    /// External operations
    /// </summary>
    [ApiController]
    [Route("ExternalOperation")]
    [Authorize]
    public class ExternalOperationsController : Controller
    {
        private readonly ITransactionSync _transactionSynchronizationService;
        private readonly ITransactionBackup _transactionBackupService;
        private readonly ILogger _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="transactionSyncService"></param>
        /// <param name="transactionBackupService"></param>
        /// <param name="logger"></param>
        public ExternalOperationsController(ILogger<ExternalOperationsController> logger,ITransactionSync transactionSyncService,ITransactionBackup transactionBackupService)
        {
            _transactionSynchronizationService = transactionSyncService;
            _transactionBackupService = transactionBackupService;
            _logger = logger;
        }

        /// <summary>
        /// Synchronize external transaction
        /// </summary>        
        /// <returns>
        /// Transactions saved
        /// </returns>
        [HttpGet("Synchronize")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDTO>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> SyncExternalTransactionsAsync()
        {
            try
            {
                _logger.LogInformation("Synchronizing transactions from azure");

                IEnumerable<TransactionDTO> externalTransactionSaved = await _transactionSynchronizationService.SyncExternalTransactionAsync();
                return Ok(externalTransactionSaved);
            }
            catch (Exception e)
            {
                _logger.LogError("Error on sync transactions {e}", e);
                return Problem(detail:e.Message);
            }
        }

        /// <summary>
        /// Create backup of all transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet("Backup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> BackupTransactionsAsync()
        {
            try
            {
                _logger.LogInformation("Backup transactions from azure");

                await _transactionBackupService.BackupDataBaseAsync();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("Error on backup transactions {e}", e);

                return Problem(detail: e.Message);
            }
        }

        /// <summary>
        /// Download latest backup file
        /// </summary>        
        [HttpGet("DownloadBackupFile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> DownloadBackupAsync()
        {
            try
            {
                _logger.LogInformation("Download transactions from azure");

                byte[] fileBytes = await _transactionBackupService.DownloadBackupFileAsync();

                return File(fileBytes, "application/octet-stream","TransactionsBackup");
            }
            catch (Exception e)
            {
                _logger.LogError("Error on download backup transactions {e}", e);

                return Problem(detail: e.Message);
            }
        }
    }
}
