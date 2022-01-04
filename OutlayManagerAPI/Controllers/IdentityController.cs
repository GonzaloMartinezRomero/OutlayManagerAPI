using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutlayManagerAPI.Model.UserAuthentication;
using OutlayManagerAPI.Services.AuthenticationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Controllers
{
    [Route("Identity")]    
    public class IdentityController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public IdentityController(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }
                
        [HttpPost("Authenticate")]        
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(AuthenticationCredentials))]
        [ProducesResponseType(500, Type= typeof(ProblemDetails))]
        [ProducesResponseType(401, Type = typeof(UnauthorizedObjectResult))]
        public IActionResult Authenticate([FromBody]UserCredential userCredential)
        {
            try 
            {
                AuthenticationCredentials authenticationCredentials = this._authenticationService.Authenticate(userCredential);
                return Ok(authenticationCredentials);
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(Exception e)
            {
                return Problem(detail: e.Message);
            }
        }
    }
}
