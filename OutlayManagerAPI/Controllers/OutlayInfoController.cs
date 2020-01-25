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
    [Route("[controller]")]
    public class OutlayInfoController : Controller
    {
        private readonly IOutlayServiceAPI service;

        public OutlayInfoController(IOutlayServiceAPI service)
        {
            this.service = service;
        }

        [Route("[action]")]
        [HttpGet]
        public List<string> TypeOutlays()
        {
            return service.GetTypeOutlays();
        }

    }
}
