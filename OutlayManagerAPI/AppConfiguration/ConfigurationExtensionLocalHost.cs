using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace OutlayManagerAPI.AppConfiguration
{
    public static class ConfigurationExtensionLocalHost
    {
        internal const string ENVIRONMENT_VARIABLE = "ASPNETCORE_ENVIRONMENT";
        internal const string PRODUCTION_ENVIRONMENT = "Production";
        internal const string DEVELOPMENT_ENVIRONMENT = "Development";

        public static void UseCurrentProcessEnvironment(this IWebHostBuilder webHostBuilder)
        {   
            string environmentHost = Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE, EnvironmentVariableTarget.Process);

            switch (environmentHost)
            {
                case PRODUCTION_ENVIRONMENT:
                    webHostBuilder.UseEnvironment(Environments.Production);
                    break;

                default:
                    webHostBuilder.UseEnvironment(Environments.Development);
                    break;
            }   
        }
    }
}
