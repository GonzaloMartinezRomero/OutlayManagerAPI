using Microsoft.AspNetCore.Mvc;
using OutlayManager.Infraestructure.Services.Abstract;
using OutlayManager.Model;

namespace OutlayManager.Api.Controllers
{
    /// <summary>
    /// PendingTransaction
    /// </summary>
    [ApiController]
    [Route("PendingTransaction")]   
    public class PendingTransactionController : Controller
    {
        private readonly ITransactionPending _pendingTransactionService;        
        private readonly ILogger _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pendingTransactionService"></param>
        public PendingTransactionController(ILogger<PendingTransactionController> logger, ITransactionPending pendingTransactionService)
        {
            _pendingTransactionService = pendingTransactionService ?? throw new ArgumentNullException(nameof(ITransactionPending));         
            _logger = logger;
        }

        /// <summary>
        /// Return saved transactions
        /// </summary>
        /// <returns></returns>
        [HttpPost("SavePendingTransactions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDTO>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public IActionResult SavePendingTransactions()
        {
            try
            {
                _logger.LogInformation("Saving pending transactions...");

                IEnumerable<TransactionDTO> pendingTransactions = _pendingTransactionService.SavePendingTransactions();
                return Ok(pendingTransactions);
            }
            catch (Exception e)
            {
                _logger.LogError("Error on pending transactions {e}", e);
                return Problem(detail:e.Message);
            }
        }
    }
}
