using OutlayManagerAPI.Model.Authentication;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Authentication.Abstract 
{ 
    public interface IAuthenticationService
    {
        Task<AuthenticationCredentials> Authenticate(UserCredential userCredentials);
    }
}
