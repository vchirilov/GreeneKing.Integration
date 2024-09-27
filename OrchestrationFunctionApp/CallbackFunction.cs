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
        [ServiceBusTrigger("%QueueName%", Connection = "QueueConnectionString")] ServiceBusReceivedMessage message)
        {
            try
            {
                _logger.LogInformation($"Received message with SequenceId: {message.ApplicationProperties["SequenceId"]}");               

                string requestBody = System.Text.Encoding.UTF8.GetString(message.Body);
                Workflow workflow = JsonConvert.DeserializeObject<Workflow>(requestBody);

                await _httpService.GetRequest(workflow);

                _logger.LogInformation($"Message body: {JsonConvert.SerializeObject(workflow)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to process message: {ex.Message}");
            }
        }
    }
}
