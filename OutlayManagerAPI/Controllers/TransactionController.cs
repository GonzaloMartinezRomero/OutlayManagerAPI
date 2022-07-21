using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OutlayManagerAPI.Controllers
{
    [ApiController]    
    [Route("Transactions")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionServices _transactionService;       

        public TransactionController(ITransactionServices transactionService)
        {
            this._transactionService = transactionService;
        }

        /// <summary>
        /// Return all transactions
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(200,Type = typeof(List<TransactionDTO>))]
        [ProducesResponseType(400,Type=typeof(BadRequestObjectResult))]
        [ProducesResponseType(401)]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [Produces("application/json")]
        public async Task<IActionResult> Transactions(int? year, int? month)
        {
            try
            {
                List<TransactionDTO> listTransactions;

                if (month.HasValue && !year.HasValue)
                    return BadRequest("Year can not be null");

                if (year == null)
                {
                    listTransactions = await this._transactionService.Transactions();
                }
                else
                {
                    int yearNotNullable = year.Value;
                    int monthNotNullable = month ?? 0;

                    listTransactions = await this._transactionService.Transactions(yearNotNullable,monthNotNullable);
                }

                return Ok(listTransactions);

            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }
        }      

        /// <summary>
        /// Get transaction by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TransactionDTO))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(400, Type = typeof(BadRequestResult))]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> Transaction([Required] uint id)
        {
            try
            {
                TransactionDTO transaction = await this._transactionService.Transaction(id);
                return Ok(transaction);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Updates a transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(TransactionDTO))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateTransaction([FromBody] TransactionDTO transaction)
        {
            try
            {
                TransactionDTO transactionUpdated = await this._transactionService.UpdateTransaction(transaction);
                return Ok(transactionUpdated);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(UInt32))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDTO transaction)
        {
            try
            {
                uint id = await this._transactionService.SaveTransaction(transaction);
                return Ok(id);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Delete transaction 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(UInt32))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(401)]        
        [Produces("application/json")]
        public async Task<IActionResult> DeleteTransaction(uint id)
        {
            try
            {
                uint deletedID = await this._transactionService.DeleteTransaction(id);

                return Ok(deletedID);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
