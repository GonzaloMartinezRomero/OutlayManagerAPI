using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OutlayManager.Authentication.Utils;
using OutlayManagerAPI.Authentication.Abstract;
using OutlayManagerAPI.Authentication.Implementation;
using System;

namespace OutlayManager.Authentication
{
    public static class Startup
    {
        public static IServiceCollection AddAuthenticationService(this IServiceCollection service, IConfiguration configuration)
        {
            string urlAuthenticationService = configuration.GetValue<string>(Constants.APP_CONFIG_KEY_IDSERVER_URL) 
                ?? throw new NullReferenceException($"{Constants.APP_CONFIG_KEY_IDSERVER_URL} not found in app config"); ;

            service.AddAuthentication("Bearer")
                 .AddJwtBearer("Bearer", opt =>
                 {
                     opt.Authority = urlAuthenticationService;
                     opt.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateAudience = false
                     };
                 });

            service.AddTransient<IAuthenticationService, IdentityService>();

            return service;
        }
    }
}
