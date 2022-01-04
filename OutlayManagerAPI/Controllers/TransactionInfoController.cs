using Microsoft.AspNetCore.Mvc;
using OutlayManagerAPI.Model;
using OutlayManagerAPI.Services.TransactionServices;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OutlayManagerAPI.Controllers
{
    [ApiController]
    [Route("TransactionInfo")]
    public class TransactionInfoController : Controller
    {
        private readonly ITransactionServices _transactionService;

        public TransactionInfoController(ITransactionServices transactionService)
        {
            this._transactionService = transactionService;
        }

        [ProducesResponseType(200, Type = typeof(List<TypeTransactionDTO>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [Route("[action]")]
        [Produces("application/json")]
        [HttpGet]
        public ActionResult<List<TypeTransactionDTO>> TransactionTypes()
        {
            try
            {
                List<TypeTransactionDTO> transactionTypeList = this._transactionService.GetMTypeTransactions();
                return Ok(transactionTypeList);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [ProducesResponseType(200, Type = typeof(List<TypeTransactionDTO>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [Route("[action]")]
        [Produces("application/json")]
        [HttpGet]
        public ActionResult<List<CodeTransactionDTO>> TransactionCodes()
        {
            try
            {
                List<CodeTransactionDTO> codeTransactionsList = this._transactionService.GetMCodeTransactions();
                return Ok(codeTransactionsList);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [ProducesResponseType(200, Type = typeof(List<int>))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [Route("[action]")]
        [Produces("application/json")]
        [HttpGet]
        public ActionResult<List<int>> YearsAvailabes()
        {
            try
            {
                List<int> yearsAvailabes = this._transactionService.GetYearsAvailabes();
                return Ok(yearsAvailabes);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
