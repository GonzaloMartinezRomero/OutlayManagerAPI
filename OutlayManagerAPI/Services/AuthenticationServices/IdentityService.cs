using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using OutlayManagerAPI.Model.UserAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Services.AuthenticationServices
{
    public class IdentityService : IAuthenticationService
    {
        private const string APP_CONFIG_KEY_IDSERVER_URL = "IdentityServerURL";
        private const string API_SCOPE = "OutlayManagerAPI";
        private readonly IConfiguration configurationApp;

        public IdentityService(IConfiguration appConfiguration)
        {
            this.configurationApp = appConfiguration;
        }

        public async Task<AuthenticationCredentials> Authenticate(UserCredential userCredentials)
        {
            string identityServerURL = configurationApp.GetValue<string>(APP_CONFIG_KEY_IDSERVER_URL);

            HttpClient httpClient = new HttpClient();

            DiscoveryDocumentResponse idServerDocument = await httpClient.GetDiscoveryDocumentAsync(identityServerURL);

            if (idServerDocument.IsError)
                throw new Exception(idServerDocument.Error);

            TokenResponse tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = idServerDocument.TokenEndpoint,
                ClientId = userCredentials.UserName,
                ClientSecret = userCredentials.Password,
                Scope = API_SCOPE
            });

            if (tokenResponse.IsError)
                throw new UnauthorizedAccessException(tokenResponse.Error);

            return new AuthenticationCredentials()
            {
                CredentialToken = tokenResponse.AccessToken
            };
        }
    }
}
