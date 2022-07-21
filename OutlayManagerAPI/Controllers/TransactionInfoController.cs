using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OutlayManagerAPI.Controllers
{
    /// <summary>
    /// Transaction info 
    /// </summary>
    [ApiController]
    [Route("TransactionInfo")]
    public class TransactionInfoController : Controller
    {
        private readonly ITransactionInfoService _transactionInfoService;
        private readonly ILogger _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="transactionInfoService"></param>
        public TransactionInfoController(ILogger<TransactionInfoController> logger,ITransactionInfoService transactionInfoService)
        {
            this._transactionInfoService = transactionInfoService;
            _logger = logger;
        }

        /// <summary>
        /// Transactions types
        /// </summary>
        /// <returns></returns>
        [HttpGet("TransactionTypes")]
        [ProducesResponseType(200, Type = typeof(List<TypeTransactionDTO>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]        
        [Produces("application/json")]
        public async Task<IActionResult> TransactionTypes()
        {
            try
            {
                _logger.LogInformation("Get transactions types");

                List<TypeTransactionDTO> transactionsTypes = await this._transactionInfoService.TransactionsTypes();
                return Ok(transactionsTypes);
            }
            catch (Exception e)
            {
                _logger.LogError("Error on get transactions types {e}", e);
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Gets years with registered transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet("YearsAvailables")]
        [ProducesResponseType(200, Type = typeof(List<int>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]        
        [Produces("application/json")]        
        public async Task<IActionResult> YearsAvailabes()
        {
            try
            {
                _logger.LogInformation("Get years availables");

                List<int> yearsAvailabes = await this._transactionInfoService.YearsAvailabes();
                return Ok(yearsAvailabes);
            }
            catch (Exception e)
            {
                _logger.LogError("Error on get years availables {e}", e);
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Gets the total amount per month and year. Show the total amount progression along the time
        /// </summary>
        /// <returns></returns>
        [HttpGet("AmountResumes")]
        [ProducesResponseType(200, Type = typeof(List<AmountResume>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> AmountResumes()
        {
            try
            {
                _logger.LogInformation("Get amount resumes");

                List<AmountResume> amountResumes = await _transactionInfoService.AmountResumes();
                return Ok(amountResumes);
            }
            catch(Exception e)
            {
                _logger.LogError("Error on get amount resumes {e}", e);
                return Problem(e.Message);
            }
        }
    }
}
