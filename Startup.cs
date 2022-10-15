using MagicAPI.Context;
using MagicAPI.IntegrationService;
using MagicAPI.IntegrationService.Interface;
using MagicAPI.Repository;
using MagicAPI.Repository.Interface;
using MagicAPI.Service;
using MagicAPI.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MtgApiManager.Lib.Service;
using VDI.API.AutoMapper;

namespace MagicAPI
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

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddScoped<IMtgServiceProvider, MtgServiceProvider>();
            services.AddScoped<Service.Interface.ICardService, CardService>();

            services.AddScoped<IMTGSDkIntegrationService, MTGSDkIntegrationService>();
            services.AddScoped<ICardMarketAPIIntegrationService, CardMarketAPIIntegrationService>();

            services.AddScoped<ICardRepository, CardRepository>();

            AutoMapperConfiguration.RegisterMappings();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MagicAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MagicAPI v1"));
            }

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
