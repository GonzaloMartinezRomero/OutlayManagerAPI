using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutlayManager.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Controllers
{
    [ApiController]
    [Route("ExternalTransaction")]
    [Authorize]
    public class ExternalTransactionController : Controller
    {
        private readonly IExternalTransactionService _transactionCodeService;

        public ExternalTransactionController(IExternalTransactionService externalTransactionService)
        {
            this._transactionCodeService = externalTransactionService;
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
        [Produces("application/json")]
        public async Task<IActionResult> DownloadExternalTransactions()
        {
            try
            {
                IEnumerable<TransactionDTO> externalTransactionSaved = await _transactionCodeService.DownloadExternalTransaction();
                return Ok(externalTransactionSaved);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }      
    }
}
