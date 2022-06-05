using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutlayManager.ExternalService.Abstract;
using OutlayManager.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Controllers
{
    [ApiController]
    [Route("ExternalOperation")]
    [Authorize]
    public class ExternalOperationsController : Controller
    {
        private readonly ITransactionSync _transactionSynchronizationService;
        private readonly ITransactionBackup _transactionBackupService;

        public ExternalOperationsController(ITransactionSync transactionSyncService,ITransactionBackup transactionBackupService)
        {
            _transactionSynchronizationService = transactionSyncService;
            _transactionBackupService = transactionBackupService;
        }

        /// <summary>
        /// Download external transaction
        /// </summary>
        /// <param name="codeTransactionDTO"></param>
        /// <returns>
        /// Transactions saved
        /// </returns>
        [HttpGet("Download")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDTO>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> SyncExternalTransactionsAsync()
        {
            try
            {
                IEnumerable<TransactionDTO> externalTransactionSaved = await _transactionSynchronizationService.SyncExternalTransactionAsync();
                return Ok(externalTransactionSaved);
            }
            catch (Exception e)
            {
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
                await _transactionBackupService.BackupDataBaseAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message);
            }
        }
    }
}
