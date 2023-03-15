using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutalyManager.Cache;
using OutlayManager.Infraestructure.Services.Abstract;
using OutlayManager.Infraestructure.Services.Implementation;
using OutlayManager.Infraestructure.Utilities;
using OutlayManagerAPI.Infraestructure.Persistence;
using OutlayManagerAPI.Infraestructure.Services.Abstract;
using OutlayManagerAPI.Infraestructure.Services.Implementation;
using OutlayManagerCore.Infraestructure.Persistence.SQLite;

namespace OutlayManager.Infraestructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOutlayMemoryCache();

            services.AddTransient<IOutlayDBContext>(impl =>
            {
                string connectionString = configuration.GetConnectionString(AppSettingConstants.CONNECTION_STRING);
                return new SQLiteContext(connectionString);
            });

            services.AddTransient<ITransactionServices, TransactionServices>();
            services.AddTransient<ITransactionCodeService, TransactionCodeService>();
            services.AddTransient<ITransactionInfoService, TransactionInfoService>();
            services.AddTransient<ITransactionPending, AzureTransactionQueue>();


            return services;
        }
    }
}
