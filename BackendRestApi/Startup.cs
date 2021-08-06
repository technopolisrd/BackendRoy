using Common.Core.Contracts.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Backend.Repository.Common;
using Common.Core.Engine;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Backend.Context.Data.Security;
using Backend.Context.Data.API;
using BackendRestApi.Helpers;
using BackendRestApi.Services.Contracts;
using BackendRestApi.Services.Services;
using BackendRestApi.Middleware;
using AutoMapper;
using System;

namespace BackendRestApi
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
            services.AddCors(options =>
            {
                options.AddPolicy("All", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            });

            //Connection Strings according to the database engine to connect.
            switch (Configuration.GetSection("SecurityEngine").Value)
            {
                case "SecuritySQLServer":
                    services.AddDbContext<SecurityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConexionSecuritySQLServer")));
                    break;
                case "SecurityMySql":
                    services.AddDbContext<SecurityContext>(options => options.UseMySQL(Configuration.GetConnectionString("ConexionSecurityMySql")));
                    break;
                case "SecuritySqlLite":
                    services.AddDbContext<SecurityContext>(options => options.UseSqlite(Configuration.GetConnectionString("ConexionSecuritySqlLite")));
                    break;
            }

            // Connection Strings according to the database engine to connect.
            switch (Configuration.GetSection("DataBaseEngine").Value)
            {
                case "SQLServer":
                    services.AddDbContextPool<BackendContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConexionSQLServer")));
                    break;
                case "MySql":
                    services.AddDbContextPool<BackendContext>(options => options.UseMySQL(Configuration.GetConnectionString("ConexionMySql")));
                    break;
                case "SqlLite":
                    services.AddDbContext<SecurityContext>(options => options.UseSqlite(Configuration.GetConnectionString("ConexionSqlLite")));
                    break;
            }

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Register the swagger generator and define a Swagger document
            // for Northwind service
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(name: "v1", info: new OpenApiInfo
                { Title = "BackendRoy Service API", Version = "v1" });
            });

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Bases
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDataRepositoryFactory, DataRepositoryFactory>();
            services.AddScoped<IBusinessEngineFactory, BusinessEngineFactory>();

            // configure DI for application services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("All");

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendRoe Service API Version 1");
                options.SupportedSubmitMethods(new[] {
                        SubmitMethod.Get, SubmitMethod.Post,
                        SubmitMethod.Put, SubmitMethod.Delete
                    });
            });
        }
    }
}