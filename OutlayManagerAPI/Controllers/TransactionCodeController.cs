using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Controllers
{
    /// <summary>
    /// Transaction code controller
    /// </summary>
    [ApiController]
    [Route("TransactionCodes")]
    public class TransactionCodeController : Controller
    {
        private readonly ITransactionCodeService _transactionCodeService;
        private readonly ILogger _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="transactionCodeService"></param>
        /// <param name="logger"></param>
        public TransactionCodeController(ITransactionCodeService transactionCodeService, ILogger<TransactionCodeController> logger)
        {
            this._transactionCodeService = transactionCodeService;
            _logger = logger;
        }

        /// <summary>
        /// Create new transaction code
        /// </summary>
        /// <param name="codeTransactionDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Int32))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [Produces("application/json")]
        public async Task<IActionResult> CreateTransactionCode([FromBody] CodeTransactionDTO codeTransactionDTO)
        {
            try
            {
                _logger.LogInformation("Create transactions codes {codeTransactionDTO}", codeTransactionDTO);

                int id = await this._transactionCodeService.AddTransactionCode(codeTransactionDTO);
                return Ok(id);
            }
            catch (Exception e)
            {
                _logger.LogError("Error on create transactions codes {e}", e);
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Delete transaction code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(Int32))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(400, Type = typeof(BadRequestResult))]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteTransactionCode([Required] uint id)
        {
            try
            {
                _logger.LogInformation("Delete transactions codes {id}", id);

                int idDeleted = await this._transactionCodeService.DeleteTransactionCode(id);
                return Ok(idDeleted);
            }
            catch (Exception e)
            {
                _logger.LogError("Error on delete transaction code {e}", e);

                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Gets transactions codes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TypeTransactionDTO>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [Produces("application/json")]
        public async Task<IActionResult> TransactionCodes()
        {
            try
            {
                _logger.LogInformation("Get transactions codes");

                List<CodeTransactionDTO> codeTransactionsList = await this._transactionCodeService.TransactionsCodes();
                return Ok(codeTransactionsList);
            }
            catch (Exception e)
            {
                _logger.LogError("Error on get transactions codes {e}", e);
                return Problem(e.Message);
            }
        }
    }
}
