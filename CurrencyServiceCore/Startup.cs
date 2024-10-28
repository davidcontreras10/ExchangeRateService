using CurrencyServiceCore.Implementations;
using CurrencyServiceCore.Models;
using DataAccess;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CurrencyServiceCore
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
			services.AddHttpClient();
			services.AddSingleton<IConfiguration>(Configuration);
			services.Configure<ProjectSettingsModel>(Configuration);
			services.AddSwaggerGen();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Implement Swagger UI",
					Description = "A simple example to Implement Swagger UI",
				});
			});

			RegisterDependencies(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Showing API V1");
			});
		}
		private static void RegisterDependencies(IServiceCollection services)
		{
			services.AddSingleton<IConnectionConfig, ConnectionStringConfig>();
			services.AddScoped<IExchangeCurrencyDataService, ExchangeCurrencyDataService>();
			services.AddScoped<IDolarColonesBccrService, DolarColonesBccrService>();
			services.AddScoped<IBccrCurrencyService, BccrCurrencyService>();
			services.AddScoped<IBccrCurrencyRepository, BccrWebApiService>();
			services.AddScoped<IBccrExchangeCache, BccrExchangeCache>();
			services.AddScoped<IBccrCodesRepository, BccrCodesRepository>();
			services.AddScoped<IBccrCodesDbCache, BccrCodesDbCache>();
			services.AddSingleton<IAppMemoryCache, AppMemoryCache>();
			services.AddSingleton<IMemoryCache, MemoryCache>();
			services.AddSingleton<IProjectSettings, ProjectSettings>();
		}
	}

}
