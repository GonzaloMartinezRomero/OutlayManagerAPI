using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OutlayManagerAPI.Services;
using OutlayManagerCore.Services;
using System;

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
            
            services.AddSingleton<IOutlayServiceAPI, OutlayServiceAPI>(config  => 
            {
                OutlayServiceAPI outlayServiceAPI = new OutlayServiceAPI(configurationServiceBD => 
                {
                    configurationServiceBD.Provider = ConfigurationServices.TypesProviders.SQLITE_ON_EF;
                    configurationServiceBD.PathConnection = Configuration.GetConnectionString("PathBDConnection");
                });

                return outlayServiceAPI;
            });

            services.AddControllers()
                    .ConfigureApiBehaviorOptions(options=> 
                    {   
                        options.ClientErrorMapping[404].Link = "https://httpstatuses.com/404";
                    });

            services.AddControllers(config =>
            {
                //Devuelve un "Not Acceptable" si el dato devuelto no tiene el parseador adecuado
                //Por defecto se tiene json, pero en este caso se le ha agregado xml, cualquier otro lanza un 406(NotAcceptabler)
                config.ReturnHttpNotAcceptable = true;

            }).AddXmlDataContractSerializerFormatters();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Outlay API Documentation", Version = "v1" });
            });

            //AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //    app.UseDeveloperExceptionPage();
            //else
            //    app.UseExceptionHandler();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Swagger configuration
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Outlays Manager API Documentation");                
            });
        }
    }
}
