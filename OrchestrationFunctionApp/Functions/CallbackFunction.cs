using System;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Services;

namespace OrchestrationFunctionApp.Functions
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

        [FunctionName("callback-function")]
        public async Task Run(
        [ServiceBusTrigger("%QueueName%", Connection = "QueueConnectionString")] ServiceBusReceivedMessage message)
        {
            try
            {
                _logger.LogInformation("Callback function has started...");               

                string requestBody = System.Text.Encoding.UTF8.GetString(message.Body);
                Workflow workflow = JsonConvert.DeserializeObject<Workflow>(requestBody);

                if (workflow.Method.ToLower() == "get")
                {
                    await _httpService.GetRequest(workflow);
                }
                else if (workflow.Method.ToLower() == "post")
                {
                    await _httpService.PostRequest(workflow);
                }

                await _httpService.GetRequest(workflow);

                _logger.LogInformation($"CallbackFunction Message Body: {JsonConvert.SerializeObject(workflow)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CallbackFunction failed to process message: {ex.Message}");
            }
        }       
    }
}
