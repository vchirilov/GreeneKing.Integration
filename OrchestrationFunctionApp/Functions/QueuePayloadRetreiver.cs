using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrchestrationFunctionApp.Options;
using Microsoft.Extensions.Options;
using Azure.Messaging.ServiceBus;
using System.Text;

namespace OrchestrationFunctionApp.Functions
{
    public class QueuePayloadRetreiver
    {
        private readonly ILogger<GatewayFunction> _logger;
        private readonly ServiceBusSettings _serviceBusSettings;

        public QueuePayloadRetreiver(ILogger<GatewayFunction> logger, IOptions<ServiceBusSettings> serviceBusSettings)
        {
            _logger = logger;
            _serviceBusSettings = serviceBusSettings.Value;
        }

        [FunctionName("queue-payload-retreiver")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string queueName = req.Query["queue"];

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                queueName = queueName ?? data?.queue;


                // Initialize queue sender            
                ServiceBusClient serviceBusClient = new ServiceBusClient(_serviceBusSettings.QueueConnectionString);
                var pipelineEventQueueSender = serviceBusClient.CreateSender(_serviceBusSettings.QueueName);                

                string responseMessage = string.IsNullOrEmpty(queueName)
                    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                    : $"Hello, {queueName}. This HTTP triggered function executed successfully.";

                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
            
        }
    }
}
