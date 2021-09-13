using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OutlayManagerAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OutlayManagerAPI.Controllers
{
    [ApiController]
    [Route("OutlayInfo")]
    public class OutlayInfoController : Controller
    {
        private readonly IOutlayServiceAPI service;

        public OutlayInfoController(IOutlayServiceAPI service)
        {
            this.service = service;
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult TypeOutlays()
        {
            try
            {
                List<string> typeOutlays = service.GetTypeOutlays();
                return Ok(typeOutlays);
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message, statusCode: 500, title: "Error while processing type outlays"); 
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult CodeList()
        {
            try
            {
                List<string> codeList = service.GetAllTransactions().Select(x=>x.DetailTransaction.Code)
                                                                    .Distinct()
                                                                    .ToList();
                return Ok(codeList);
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message, statusCode: 500, title: "Error while processing code list transactions");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult YearsAvailabes()
        {
            try
            {
                List<int> yearsAvailabes = service.GetAllTransactions().Select(x => x.Date.Year)
                                                                       .Distinct()
                                                                       .OrderBy(x=>x)
                                                                       .ToList();
                return Ok(yearsAvailabes);
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message, statusCode: 500, title: "Error while processing years availables");
            }
        }
    }
}
