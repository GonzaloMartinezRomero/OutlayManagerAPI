using Microsoft.AspNetCore.Mvc;
using OutlayManagerAPI.Authentication.Abstract;
using OutlayManagerAPI.Model.Authentication;
using System;
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
        [ProducesResponseType(401)]
        public async Task<IActionResult> Authenticate([FromBody]UserCredential userCredential)
        {
            try 
            {
                AuthenticationCredentials authenticationCredentials = await this._authenticationService.Authenticate(userCredential);
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
