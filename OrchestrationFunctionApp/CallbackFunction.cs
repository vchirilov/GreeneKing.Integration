using System;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Services;

namespace OrchestrationFunctionApp
{
    public class CallbackFunction
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<CallbackFunction> _logger;

        public CallbackFunction(IHttpService httpService, ILogger<CallbackFunction> logger)
        {
            _httpService = httpService;
            _logger = logger;
        }

        [FunctionName("CallbackFunction")]
        public async Task Run(
        [ServiceBusTrigger("pipeline-event", Connection = "QueueConnectionString")] ServiceBusReceivedMessage message)
        {
            try
            {
                _logger.LogInformation($"Received message with SequenceId: {message.ApplicationProperties["SequenceId"]}");

                await _httpService.SendGetRequest();

                string requestBody = System.Text.Encoding.UTF8.GetString(message.Body);
                Workflow workflow = JsonConvert.DeserializeObject<Workflow>(requestBody);

                // Simulate processing
                _logger.LogInformation($"Message body: {JsonConvert.SerializeObject(workflow)}");
                await Task.Delay(5000);  // Simulate some processing work
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to process message: {ex.Message}");
            }
        }
    }
}
