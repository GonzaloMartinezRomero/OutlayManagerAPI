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
                    //Te permite cargar la config desde el proceso que lo ejecuta en lugar de las variables de la maquina
                    webBuilder.UseCurrentProcessEnvironment();
                    webBuilder.UseStartup<Startup>();                   
                });
    }
}
