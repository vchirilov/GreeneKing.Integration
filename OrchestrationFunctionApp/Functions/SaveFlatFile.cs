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
    public class SaveFlatFile
    {
        private readonly ILogger<SaveFlatFile> _logger;
        private readonly IEventProcessor _eventProcessor;

        public SaveFlatFile(ILogger<SaveFlatFile> logger, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
        }


        [FunctionName("save-flat-file")]
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
                _logger.LogError($"Function [save-flat-file] has failed with error message: {ex.Message}");
            }
        }
    }
}