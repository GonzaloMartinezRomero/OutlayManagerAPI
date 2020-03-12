using System;
using System.Collections.Generic;
using System.Linq;
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
        /*
         IActionResult te permite encapsular fallos + datos, si devuelvo el objeto, solo obtengo los datos   
         */
        public IActionResult GetAllTransactions()
        {
            //Get all transaction from outlay core
            List<Transaction> listTransactions = new List<Transaction>();
            listTransactions.AddRange(service.GetAllTransactions());

            //Parse for return to dto standar
            List<WSTransaction> listWSTransaction = new List<WSTransaction>();

            foreach (Transaction transactionFromCore in listTransactions)
                listWSTransaction.Add(CastObject.ToWSTransaction(transactionFromCore));

            return Ok(listWSTransaction);
        }

        [HttpGet]
        public IActionResult GetTransactions(int year, int month)
        {
            //Get all transaction from outlay core
            List<Transaction> listTransactions = new List<Transaction>();
            listTransactions.AddRange(service.GetTransactions(year, month));

            //Parse for return to dto standar
            List<WSTransaction> listWSTransaction = new List<WSTransaction>();

            foreach (Transaction transactionFromCore in listTransactions)
                listWSTransaction.Add(CastObject.ToWSTransaction(transactionFromCore));

            return (listWSTransaction.Count == 0) ? (IActionResult)NotFound() : Ok(listWSTransaction);
        }

        [HttpGet("{id}")]
        public Transaction GetTransaction(int id)
        {
            return service.GetTransaction(id);
        }

        [HttpPut]
        public StateInfo UpdateTransaction([FromBody]WSTransaction transaction)
        {
            return service.UpdateTransaction(transaction);
        }

        [HttpPost]
        public StateInfo AddTransaction([FromBody]WSTransaction transaction)
        {
           return service.AddTransaction(transaction);
        }
               
        [HttpDelete("{id}")]
        public StateInfo DeleteTransaction(int id)
        {
            return service.DeleteTransaction(id);
        }
    }
}
