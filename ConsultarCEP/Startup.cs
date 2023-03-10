using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using ConsultarCEP.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using EvolveDb;
using ConsultarCEP.Repository;
using ConsultarCEP.Repository.Implementation;
using ConsultarCEP.Business;
using ConsultarCEP.Business.Implementation;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;

namespace ConsultarCEP
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Enviroment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Enviroment = environment;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();

            }));

            services.AddControllers();

            var connection = Configuration["MSSQLServerConnection:MSSQLServerConnectionString"];
            services.AddDbContext<MSSQLContext>(options => options.UseSqlServer(connection));
            if (Enviroment.IsDevelopment())
            {
                MigrateDataBase(connection);
                
            }

            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Consultar CEP",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Wesley Luz",
                        Url = new Uri("https://github.com/wesleyluz")
                    }
                });
            });
            services.AddScoped<IRepository, RepositoryImp>();
            services.AddScoped<ICEPBusiness, CEPBusinessImp>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors();

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Consultar CEP - v1");
            });

            var option = new RewriteOptions();
            app.UseRewriter(option);
            option.AddRedirect("^$", "swagger");
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}");
                endpoints.MapRazorPages();
            });
        }
        private void MigrateDataBase(string connection) 
        {
            try
            {
                var evolveConnection = new SqlConnection(connection);
                var evolve = new Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset" },
                    IsEraseDisabled = true,
                };
                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("DataBase Migration Falied", ex);
                throw;
            }
        }
    }
}
