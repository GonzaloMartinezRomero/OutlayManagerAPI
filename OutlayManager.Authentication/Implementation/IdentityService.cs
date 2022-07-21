using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using OutlayManager.Authentication.Utils;
using OutlayManagerAPI.Authentication.Abstract;
using OutlayManagerAPI.Model.Authentication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Authentication.Implementation
{
    internal class IdentityService : IAuthenticationService
    {
        private const string API_SCOPE = "OutlayManagerAPI";
        private readonly IConfiguration configurationApp;

        public IdentityService(IConfiguration appConfiguration)
        {
            this.configurationApp = appConfiguration;
        }

        public async Task<AuthenticationCredentials> Authenticate(UserCredential userCredentials)
        {
            string identityServerURL = configurationApp.GetValue<string>(Constants.APP_CONFIG_KEY_IDSERVER_URL) 
                ?? throw new NullReferenceException($"{Constants.APP_CONFIG_KEY_IDSERVER_URL} not found in app config");

            HttpClient httpClient = new HttpClient();

            DiscoveryDocumentResponse idServerDocument = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest() 
            { 
                Address= identityServerURL,
                Policy = new DiscoveryPolicy() { RequireHttps = false}
            });

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
