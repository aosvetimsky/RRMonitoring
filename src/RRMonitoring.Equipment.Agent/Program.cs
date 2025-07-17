using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RRMonitoring.Equipment.Agent.Extensions;

namespace RRMonitoring.Equipment.Agent;

internal class Program
{
	public static Task Main(string[] args)
	{
		return Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((hostingContext, config) =>
			{
				config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
						optional: true, reloadOnChange: true)
					.AddUserSecrets<Program>()
					.AddEnvironmentVariables();
			})
			.ConfigureServices((hostContext, services) =>
			{
				var configuration = hostContext.Configuration;

				services
					.AddAgentReferences(configuration, Assembly.GetExecutingAssembly())
					.AddScanSteps(Assembly.GetExecutingAssembly());
			})
			.ConfigureLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddConsole();
			})
			.Build()
			.RunAsync();
	}
}
