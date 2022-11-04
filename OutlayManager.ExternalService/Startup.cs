using Microsoft.Extensions.DependencyInjection;
using OutlayManager.ExternalService.BackupDataBaseService.Abstract;
using OutlayManager.ExternalService.BackupDataBaseService.Implementation;
using OutlayManager.ExternalService.TransactionBus.Abstract;
using OutlayManager.ExternalService.TransactionBus.Implementation;

namespace OutlayManager.ExternalService
{
    public static class Startup
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddTransient<ITransactionServiceBus, AzureServiceBus>();
            services.AddTransient<ITransactionBackup, TransactionBackupService>();

            return services;
        }
    }
}
