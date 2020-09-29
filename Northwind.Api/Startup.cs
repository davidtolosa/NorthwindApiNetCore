
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Northwind.Api.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Northwind.Api.Repository;
using AutoMapper;

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
                options.UseSqlServer("Server=DESKTOP-1INVORS;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true"));

            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddSwaggerGen(configuration => {
                configuration.EnableAnnotations();
                configuration.CustomSchemaIds(type => type.ToString());
            });

            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger().UseSwaggerUI(config => {
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
