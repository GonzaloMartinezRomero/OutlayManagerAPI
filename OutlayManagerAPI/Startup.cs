using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OutlayManager.Authentication;
using OutlayManager.ExternalService;
using OutlayManager.Infraestructure;
using System.Collections.Generic;

namespace OutlayManagerAPI
{
    public class Startup
    {
        private const string URL_CORS_HOST_KEY = "HostWeb";
        private const string OUTLAY_MANAGER_CORS_POLICY = "OutlayManagerWebClientCors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            services.AddInfraestructure(Configuration);
            services.AddAuthenticationService(Configuration);
            services.AddExternalServices();

            services.AddCors(corsSetting => corsSetting.AddPolicy(OUTLAY_MANAGER_CORS_POLICY, policy =>
            {
                policy.WithOrigins(Configuration[URL_CORS_HOST_KEY]);
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            }));

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Outlay API Documentation", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization scheme"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {           
            app.UseRouting();
            app.UseCors(OUTLAY_MANAGER_CORS_POLICY);
            app.UseAuthentication();
            app.UseAuthorization();

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
