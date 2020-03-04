using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Volunteer.Infrastructure.Database;

namespace Volunteer.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    @"log\log.txt",
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .CreateLogger();
            var host = CreateWebHostBuilder(args).Build();
            if (args?.Length>0 && args[0]?.Contains("/seed") == true)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var service = scope.ServiceProvider;
                    var loggerFactory = service.GetRequiredService<ILoggerFactory>();
                    try
                    {
                        var myContext = service.GetRequiredService<VolunteerContext>();
                        VolunteerContextSeed.InitData(myContext);
                    }
                    catch (Exception e)
                    {
                        var logger = loggerFactory.CreateLogger<Program>();
                        logger.LogError(e, "初始化数据Error.");
                    }
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    if (context.HostingEnvironment.IsProduction())
                    {
                        // apollo  输出在 控制台
                        Com.Ctrip.Framework.Apollo.Logging.LogManager.Provider = new ConsoleLoggerProvider(Com.Ctrip.Framework.Apollo.Logging.LogLevel.Trace);
                        config.AddApollo(config.Build().GetSection("apollo"))
                            .AddDefault();
                    }
                })
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
