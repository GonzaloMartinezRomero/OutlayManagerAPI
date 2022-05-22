using Microsoft.Extensions.DependencyInjection;
using OutalyManager.Cache.Abstract;
using OutalyManager.Cache.Implementation;

namespace OutalyManager.Cache
{
    public static class Startup
    {
        public static IServiceCollection AddOutlayMemoryCache(this IServiceCollection service)
        {
            service.AddSingleton<IOutlayManagerCache, OutlayManagerCacheService>();

            return service;
        }
    }
}
