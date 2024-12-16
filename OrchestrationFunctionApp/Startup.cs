using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchestrationFunctionApp.Options;
using OrchestrationFunctionApp.Persistence;
using OrchestrationFunctionApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(OrchestrationFunctionApp.Startup))]
namespace OrchestrationFunctionApp
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddTransient<IServiceBroker, ServiceBroker>();
            builder.Services.AddHttpClient();

            builder.Services.AddOptions<ServiceBusSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("ServiceBus").Bind(settings);
                });
            
            builder.Services.AddDbContext<GreeeKingMessageBusContext>(options =>
            {
                //var config = builder.Configuration;
                //var connectionString = config.GetConnectionString("GreeneKingConnectionString");
                options.UseSqlServer("Data Source=51.12.52.30;Initial Catalog=GreeeKingMessageBus;Persist Security Info=True;User ID=sa;Password=spartak_1; Encrypt=False");
            });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);

            var context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }
    }
}
