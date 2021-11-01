using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OutlayManagerAPI.Services.TransactionServices;
using OutlayManagerCore.Persistence.DataBase.EFWithSQLite;

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
            services.AddDbContext<SQLiteContext>(opt =>
            {
                opt.UseSqlite(Configuration.GetConnectionString("PathBDConnection"));
            });

            services.AddTransient<ITransactionServices, SQLiteTransactionServices>();            

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
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //    app.UseDeveloperExceptionPage();
            //else
            //    app.UseExceptionHandler();

            app.UseRouting();

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
