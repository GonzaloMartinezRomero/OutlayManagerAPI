using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutlayManager.ExternalService.Abstract;
using OutlayManager.ExternalService.Implementation;
using OutlayManager.Infraestructure.Services.Abstract;
using OutlayManager.Infraestructure.Services.Implementation;
using System;

namespace OutlayManager.ExternalService
{
    public static class Startup
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddTransient<ITransactionSync, TransactionSyncService>();
            services.AddTransient<ITransactionBackup, TransactionBackupService>();

            return services;
        }
    }
}
