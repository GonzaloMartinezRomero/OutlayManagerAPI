using Microsoft.AspNetCore.Mvc;
using OutlayManagerAPI.Model;
using OutlayManagerAPI.Services.TransactionCodeService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Controllers
{
    [ApiController]
    [Route("TransactionCodes")]
    public class TransactionCodeController : Controller
    {
        private readonly ITransactionCodeService _transactionCodeService;

        public TransactionCodeController(ITransactionCodeService transactionCodeService)
        {
            this._transactionCodeService = transactionCodeService;
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
                int id = await this._transactionCodeService.AddTransactionCode(codeTransactionDTO);
                return Ok(id);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Delete transaction code
        /// </summary>
        /// <param name="Id"></param>
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
                int idDeleted = await this._transactionCodeService.DeleteTransactionCode(id);
                return Ok(idDeleted);
            }
            catch (Exception e)
            {
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
                List<CodeTransactionDTO> codeTransactionsList = await this._transactionCodeService.TransactionsCodes();
                return Ok(codeTransactionsList);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
