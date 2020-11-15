using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OutlayManagerAPI.Model;
using OutlayManagerAPI.ModelBinder;
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
        private readonly IMapper mapper;

        public OutlayController(IOutlayServiceAPI service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet("All")]
        [ProducesResponseType(200,Type = typeof(List<WSTransaction>))]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public ActionResult<List<WSTransaction>> GetAllTransactions()
        {
            try
            {
                //Get all transaction from outlay core
                List<Transaction> listTransactions = new List<Transaction>(service.GetAllTransactions());

                List<WSTransaction> listWSTransaction = mapper.Map<List<WSTransaction>>(listTransactions);

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
                    listWSTransaction.Add(MapperObject.ToWSTransaction(transactionFromCore));

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

        /// <summary>
        /// .../api/(12,13,14,15)
        /// </summary>
        /// <param name="transactionsIDs"></param>
        /// <returns></returns>
        [HttpGet("({listTransactionIDs})")]
        [ProducesResponseType(200, Type = typeof(List<WSTransaction>))]
        [ProducesResponseType(500)]
        public IActionResult GetTransacionFromIDsList([ModelBinder(BinderType = typeof(TransactionListModelBinder),Name ="listTransactionIDs")] 
                                                       IEnumerable<int> transactionsIDs)
        {
            //El ModelBinder te permite transformar los parametros de entrada antes de que sean mapeados a los argumentos
            //de la funcion del controller

            try
            {
                List<Transaction> allTransactions = service.GetAllTransactions();
                HashSet<int> setIDsRequested = new HashSet<int>(transactionsIDs ?? new List<int>());

                IEnumerable<Transaction> transactionsFiltered = allTransactions.Where(x => setIDsRequested.Contains(Convert.ToInt32(x.ID)));

                List<WSTransaction> transactionsDTOs = mapper.Map<List<WSTransaction>>(transactionsFiltered);

                return Ok(transactionsDTOs);
            }
            catch(Exception e)
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
