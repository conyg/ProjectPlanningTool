using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace TeamBins.Web
{
    public class Program
    {
		static NLog.Logger logger;

		public static void Main(string[] args)
        {



			// NLog: setup the logger first to catch all errors
			logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			try
			{
				logger.Debug("Init BugTracker application on " + Environment.MachineName + " - "
					+ Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
				CreateWebHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				//NLog: catch setup errors
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
				NLog.LogManager.Shutdown();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args) // <-- Logging Added Here
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					logging.AddDebug();
					logging.SetMinimumLevel(LogLevel.Trace);
				})
			.UseNLog()  // NLog: setup NLog for Dependency injection
			.UseStartup<Startup>();
		
    }
}
