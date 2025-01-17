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
    public class SaveEmptyEvent
    {
        private readonly ILogger<SaveEmptyEvent> _logger;
        private readonly IEventProcessor _eventProcessor;

        public SaveEmptyEvent(ILogger<SaveEmptyEvent> logger, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
        }


        [FunctionName("save-empty-event")]
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
