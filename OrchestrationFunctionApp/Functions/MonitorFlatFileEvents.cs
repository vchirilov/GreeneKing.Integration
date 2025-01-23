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
    public class MonitorFlatFileEvents
    {
        private readonly ILogger<MonitorFlatFileEvents> _logger;
        private readonly IEventProcessor _eventProcessor;

        public MonitorFlatFileEvents(ILogger<MonitorFlatFileEvents> logger, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
        }


        [FunctionName("monitor-flat-file-events")]
        public async Task Run(
            [ServiceBusTrigger("sbq-event-flat-file", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message,
            ILogger log)
        {
            try
            {
                _logger.LogInformation($"sbq-event-flat-file: {message}");

                await _eventProcessor.SaveMessageAsync<MsgFlatFileModel>(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Function [save-flat-file] has failed with error message: {ex}");
            }
        }
    }
}