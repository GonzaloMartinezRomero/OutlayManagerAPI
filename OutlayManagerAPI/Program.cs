using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OutlayManagerAPI.AppConfiguration;

namespace OutlayManagerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseCurrentProcessEnvironment();

                    //Default port
                    webBuilder.UseUrls("http://*:5024");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
