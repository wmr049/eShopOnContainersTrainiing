using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.eShopOnContainers.Services.Marketing.API;
using Microsoft.eShopOnContainers.Services.Marketing.API.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Marketing.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
           .MigrateDbContext<MarketingContext>((context, services) =>
           {
               var logger = services.GetService<ILogger<MarketingContextSeed>>();

               new MarketingContextSeed()
                   .SeedAsync(context, logger)
                   .Wait();
           }).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseApplicationInsights()
            
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseWebRoot("Pics")
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                config.AddEnvironmentVariables();
            })
            .ConfigureLogging((hostingContext, builder) =>
            {
                builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddDebug();
            })
            .UseApplicationInsights()
            .UseUrls(urls: "http://localhost:5110")
            .Build();
    }
}
