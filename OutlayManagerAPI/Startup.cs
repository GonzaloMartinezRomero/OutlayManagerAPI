using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OutlayManagerAPI.Persistence;
using OutlayManagerAPI.Services.AuthenticationServices;
using OutlayManagerAPI.Services.TransactionCodeService;
using OutlayManagerAPI.Services.TransactionInfoServices;
using OutlayManagerAPI.Services.TransactionServices;
using OutlayManagerAPI.Utilities;
using OutlayManagerCore.Persistence.SQLite;
using System.Collections.Generic;

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
            services.AddTransient<IDBEntityContext>(impl=> 
            {
                string connectionString = Configuration.GetValue<string>(ApplicationConstants.CONNECTION_STRING_CONFIG_KEY);
                return new SQLiteContext(connectionString);
            });
            services.AddTransient<ITransactionServices, TransactionServices>();
            services.AddTransient<ITransactionCodeService, TransactionCodeService>();
            services.AddTransient<ITransactionInfoService, TransactionInfoService>();
            services.AddTransient<IAuthenticationService, IdentityService>();            

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

            services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", opt =>
                    {
                        opt.Authority = "https://localhost:6001";
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false
                        };
                    });    

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();                    
                });
            });

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

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
