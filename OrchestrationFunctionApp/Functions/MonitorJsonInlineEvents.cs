using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Services;

namespace OrchestrationFunctionApp.Functions
{
    public class MonitorJsonInlineEvents
    {
        private readonly ILogger<MonitorJsonInlineEvents> _logger;
        private readonly IEventProcessor _serviceBroker;

        public MonitorJsonInlineEvents(ILogger<MonitorJsonInlineEvents> logger, IEventProcessor serviceBroker)
        {
            _logger = logger;
            _serviceBroker = serviceBroker;
        }


        [FunctionName("monitor-json-inline-events")]
        public async Task Run(
            [ServiceBusTrigger("sbq-event-json-inline", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message,
            ILogger log)
        {
            try
            {
                _logger.LogInformation($"save-json-inline-messages: {message}");

                await _serviceBroker.SaveMessageAsync<MsgInlineJsonModel>(message);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Function [save-json-inline-messages] has failed with error message: {ex}");
            }
            
        }
    }
}
