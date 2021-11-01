using Microsoft.AspNetCore.Mvc;
using OutlayManagerAPI.Model;
using OutlayManagerAPI.Services.TransactionServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OutlayManagerAPI.Controllers
{
    [ApiController]    
    [Route("Transaction")] 
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionServices _transactionService;       

        public TransactionController(ITransactionServices transactionService)
        {
            this._transactionService = transactionService;
        }

        [HttpGet("All")]
        [ProducesResponseType(200,Type = typeof(List<TransactionDTO>))]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public ActionResult<List<TransactionDTO>> GetAllTransactions()
        {
            try
            {   
                List<TransactionDTO> listTransactions = new List<TransactionDTO>(_transactionService.GetAllTransactions());

                return Ok(listTransactions);

            }catch(Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TransactionDTO>))]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public ActionResult GetTransactions([Required]int year,[Required] int month)
        {
            try
            {
                if (!(year > 2000 && month >= 1 && month <= 12))
                    return BadRequest();

                //Get all transaction from outlay core
                List<TransactionDTO> listTransactions = _transactionService.GetTransactions(year, month);

                return Ok(listTransactions);

            }catch(Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TransactionDTO))]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public ActionResult GetTransaction([Required]int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                uint idParsed = (uint)id;

                TransactionDTO transaction = this._transactionService.GetTransaction(idParsed);
                return Ok(transaction);
            }
            catch (Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }            
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public ActionResult UpdateTransaction([FromBody]TransactionDTO transaction)
        {
            try
            {
                this._transactionService.UpdateTransaction(transaction);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            } 
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public ActionResult AddTransaction([FromBody]TransactionDTO transaction)
        {
            try
            {
                this._transactionService.SaveTransaction(transaction);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }
               
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult DeleteTransaction(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                uint idParsed = (uint)id;

                this._transactionService.DeleteTransaction(idParsed);
                return Ok();

            }catch(Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }

        [HttpPost("TransactionCode")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public ActionResult AddTransactionCode([FromBody]CodeTransactionDTO codeTransactionDTO)
        {
            try
            {
                this._transactionService.AddTransactionCode(codeTransactionDTO);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }

        [HttpDelete("TransactionCode/{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public ActionResult DeleteTransactionCode(int Id)
        {
            try
            {
                if (Id <= 0)
                    return BadRequest();

                uint idParsed = (uint)Id;

                this._transactionService.DeleteTransactionCode(idParsed);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }

    }
}
