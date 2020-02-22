using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OutlayManagerAPI.Services;
using OutlayManagerCore.Services;

namespace OutlayManagerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
                Singleton which creates a single instance throughout the application. It creates the instance for the first time and reuses the same object in the all calls.

                Scoped lifetime services are created once per request within the scope. It is equivalent to Singleton in the current scope. 
                eg. in MVC it creates 1 instance per each http request but uses the same instance in the other calls within the same web request.

                Transient lifetime services are created each time they are requested. This lifetime works best for lightweight, stateless services. 
             */

            services.AddSingleton<IOutlayServiceAPI, OutlayServiceAPI>();
         
            services.AddControllers()
                    .ConfigureApiBehaviorOptions(options=> 
                    {
                        options.ClientErrorMapping[404].Link = "https://httpstatuses.com/404";
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
          
            //app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Configuration for connect to BD
            app.ApplicationServices.GetService<IOutlayServiceAPI>().ConfigureOutlayServices(y =>
            {
                y.Provider = ConfigurationServices.TypesProviders.SQLITE_ON_EF;
                y.PathConnection = Configuration.GetConnectionString("PathBDConnection");
               
            }).InitialiceComponents();
        }
    }
}
