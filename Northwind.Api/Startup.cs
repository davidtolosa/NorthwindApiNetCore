using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Northwind.Api.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Northwind.Api.Repository;
using AutoMapper;
using Northwind.Api.Swagger;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using System;

namespace Northwind.Api
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
            services.AddControllers();
            services.AddDbContext<NorthwindDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Sql")));

            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddSwaggerGen(configuration => {
                configuration.EnableAnnotations();
                configuration.CustomSchemaIds(type => type.ToString());
                //configuration.DocumentFilter<RemoveDefinitionDocumentFilter>();
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddProblemDetails(opt => {
                opt.IncludeExceptionDetails = (ctx, ex) => false;
                opt.ShouldLogUnhandledException = (ctx, ex, details) => true;
                opt.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();
            
            app.UseSwagger()
               .UseSwaggerUI(config => {
                config.SwaggerEndpoint("/swagger/v1/swagger.json","NorthwindApi V1.0");
                config.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
