using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Persistence.Entities;
using OrchestrationFunctionApp.Services;

namespace OrchestrationFunctionApp.Functions
{
    public class SaveJsonInlineMessages
    {
        private readonly ILogger<SaveJsonInlineMessages> _logger;
        private readonly IServiceBroker _serviceBroker;

        public SaveJsonInlineMessages(ILogger<SaveJsonInlineMessages> logger, IServiceBroker serviceBroker)
        {
            _logger = logger;
            _serviceBroker = serviceBroker;
        }


        [FunctionName("save-json-inline")]
        public void Run(
            [ServiceBusTrigger("sbq-event-json-inline", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message,
            ILogger log)
        {
            try
            {
                _logger.LogInformation($"save-json-inline-messages: {message}");

                _serviceBroker.SaveMessageAsync<MsgInlineJsonModel>(message).Wait();
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Function [save-json-inline-messages] has failed with error message: {ex.Message}");
            }
            
        }
    }
}
