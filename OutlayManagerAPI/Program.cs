using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OutlayManagerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)// Enable appsetting.{ENVIRONMENT}.json
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();                   
                });
    }
}
