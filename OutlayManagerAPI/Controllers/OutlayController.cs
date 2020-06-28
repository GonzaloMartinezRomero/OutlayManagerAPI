using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OutlayManagerAPI.Model;
using OutlayManagerAPI.Services;
using OutlayManagerAPI.Utilities;
using OutlayManagerCore.Model.ResultInfo;
using OutlayManagerCore.Model.Transaction;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OutlayManagerAPI.Controllers
{
    [ApiController]
    //[Route("[controller]")] -> En caso de tener que refactorizar, el hecho de cambiar el nombre puede producir problemas
    [Route("Outlay")]
    public class OutlayController : ControllerBase
    {
        private readonly IOutlayServiceAPI service;

        public OutlayController(IOutlayServiceAPI service)
        {
            this.service = service;
        }

        [HttpGet("All")]
        [ProducesResponseType(200,Type = typeof(List<WSTransaction>))]
        [ProducesResponseType(500)]
        public IActionResult GetAllTransactions()
        {
            try
            {
                //Get all transaction from outlay core
                List<Transaction> listTransactions = new List<Transaction>();
                listTransactions.AddRange(service.GetAllTransactions());

                //Parse for return to dto standar
                List<WSTransaction> listWSTransaction = new List<WSTransaction>();

                foreach (Transaction transactionFromCore in listTransactions)
                    listWSTransaction.Add(CastObject.ToWSTransaction(transactionFromCore));

                return Ok(listWSTransaction);

            }catch(Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<WSTransaction>))]
        [ProducesResponseType(500)]
        public IActionResult GetTransactions([Required]int year,[Required] int month)
        {
            try
            {
                //Get all transaction from outlay core
                List<Transaction> listTransactions = new List<Transaction>();
                listTransactions.AddRange(service.GetTransactions(year, month));

                //Parse for return to dto standar
                List<WSTransaction> listWSTransaction = new List<WSTransaction>();

                foreach (Transaction transactionFromCore in listTransactions)
                    listWSTransaction.Add(CastObject.ToWSTransaction(transactionFromCore));

                return Ok(listWSTransaction);

            }catch(Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Transaction))]
        [ProducesResponseType(500)]
        public IActionResult GetTransaction([Required]int id)
        {
            try
            {
                Transaction transaction = service.GetTransaction(id);
                return Ok(transaction);
            }
            catch (Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }            
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(StateInfo))]
        [ProducesResponseType(500)]
        public IActionResult UpdateTransaction([FromBody]WSTransaction transaction)
        {
            try
            {
                StateInfo result = service.UpdateTransaction(transaction);
                return Ok(result);
            }
            catch(Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            } 
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(StateInfo))]
        [ProducesResponseType(500)]
        public IActionResult AddTransaction([FromBody]WSTransaction transaction)
        {
            try
            {
                StateInfo resultInfo =  service.AddTransaction(transaction);
                return Ok(resultInfo);
            }
            catch (Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }
               
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(StateInfo))]
        [ProducesResponseType(500)]
        public IActionResult DeleteTransaction(int id)
        {
            try
            {  
                StateInfo stateInfo = service.DeleteTransaction(id);
                return Ok(stateInfo);

            }catch(Exception e)
            {
                return StatusCode(statusCode: 500, e.Message);
            }
        }
    }
}
