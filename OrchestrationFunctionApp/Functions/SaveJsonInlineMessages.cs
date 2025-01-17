using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Services;

namespace OrchestrationFunctionApp.Functions
{
    public class SaveJsonInlineMessages
    {
        private readonly ILogger<SaveJsonInlineMessages> _logger;
        private readonly IEventProcessor _serviceBroker;

        public SaveJsonInlineMessages(ILogger<SaveJsonInlineMessages> logger, IEventProcessor serviceBroker)
        {
            _logger = logger;
            _serviceBroker = serviceBroker;
        }


        [FunctionName("save-json-inline")]
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
                _logger.LogError($"Function [save-json-inline-messages] has failed with error message: {ex.Message}");
            }
            
        }
    }
}
