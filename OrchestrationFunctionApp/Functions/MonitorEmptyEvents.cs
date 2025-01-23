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
    public class MonitorEmptyEvents
    {
        private readonly ILogger<MonitorEmptyEvents> _logger;
        private readonly IEventProcessor _eventProcessor;

        public MonitorEmptyEvents(ILogger<MonitorEmptyEvents> logger, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
        }


        [FunctionName("monitor-empty-event-events")]
        public async Task Run(
            [ServiceBusTrigger("sbq-event-empty", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message,
            ILogger log)
        {
            try
            {
                _logger.LogInformation($"sbq-event-empty: {message}");

                await _eventProcessor.SaveMessageAsync<MsgEmptyEventModel>(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Function [save-empty-event] has failed with error message: {ex}");
            }

        }
    }

}
