using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Services;

namespace OrchestrationFunctionApp.Functions
{
    public class MonitorJsonFileEvents
    {
        private readonly ILogger<MonitorJsonFileEvents> _logger;
        private readonly IEventProcessor _eventProcessor;

        public MonitorJsonFileEvents(ILogger<MonitorJsonFileEvents> logger, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
        }


        [FunctionName("monitor-json-file-events")]
        public async Task Run(
            [ServiceBusTrigger("sbq-event-json-file", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message,
            ILogger log)
        {
            try
            {
                _logger.LogInformation( $"sbq-event-json-file: {message}");

                await _eventProcessor.SaveMessageAsync<MsgJsonFileModel>(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Function [save-json-file] has failed with error message: {ex}");
            }

        }
    }
}
