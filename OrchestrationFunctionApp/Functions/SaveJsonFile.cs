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
    public class SaveJsonFile
    {
        private readonly ILogger<SaveJsonFile> _logger;
        private readonly IEventProcessor _eventProcessor;

        public SaveJsonFile(ILogger<SaveJsonFile> logger, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
        }


        [FunctionName("save-json-file")]
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
