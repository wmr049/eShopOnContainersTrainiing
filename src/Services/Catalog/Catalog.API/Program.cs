﻿using Catalog.API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace Microsoft.eShopOnContainers.Services.Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<CatalogContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var settings = services.GetService<IOptions<CatalogSettings>>();
                    var logger = services.GetService<ILogger<CatalogContextSeed>>();

                    new CatalogContextSeed()
                    .SeedAsync(context, env, settings, logger)
                    .Wait();
                })                
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .UseStartup<Startup>()
                .UseApplicationInsights()                
                .UseContentRoot(Directory.GetCurrentDirectory())                
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
                .Build();
    }
}