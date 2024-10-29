using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyServiceCore
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
				Host.CreateDefaultBuilder(args)
					.ConfigureLogging(logging =>
					{
						logging.ClearProviders(); // Optional: Clears default logging providers
						logging.AddConsole();     // Adds console logging
					})
					.ConfigureWebHostDefaults(webBuilder =>
					{
						webBuilder.UseStartup<Startup>();
					});
	}
}
