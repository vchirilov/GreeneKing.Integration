using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Messaging.ServiceBus;
using OrchestrationFunctionApp.Models;
using System.Linq;
using Microsoft.Extensions.Options;
using OrchestrationFunctionApp.Options;

namespace OrchestrationFunctionApp.Functions
{
    public class GatewayFunction
    {
        private readonly ILogger<GatewayFunction> _logger;
        private readonly ServiceBusSettings _serviceBusSettings;

        public GatewayFunction(ILogger<GatewayFunction> logger, IOptions<ServiceBusSettings> serviceBusSettings)
        {
            _logger = logger;
            _serviceBusSettings = serviceBusSettings.Value;
        }

        [FunctionName("gateway-function")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("Gateway function has started...");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                // Initialize queue sender            
                ServiceBusClient serviceBusClient = new ServiceBusClient(_serviceBusSettings.QueueConnectionString);
                var pipelineEventQueueSender = serviceBusClient.CreateSender(_serviceBusSettings.QueueName);

                // Deserialize payload into PipelineDescriptor
                PipelineDescriptor pipelineDescriptor = JsonConvert.DeserializeObject<PipelineDescriptor>(requestBody);

                var workflowsOrderedBy = pipelineDescriptor.Pipeline.OrderBy(x => x.OrderId);

                foreach (var workflow in workflowsOrderedBy)
                {
                    await SendMessage(pipelineEventQueueSender, workflow);
                }

                string responseMessage = "Sent with success.";
                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GatewayFunction failed to process message: {ex.Message}");
                return new BadRequestObjectResult(ex);
            }           

        }

        private static async Task SendMessage(ServiceBusSender pipelineEventQueueSender, Workflow workflow)
        {
            var jsonWorkflow = JsonConvert.SerializeObject(workflow);

            ServiceBusMessage message = new ServiceBusMessage(jsonWorkflow);
            message.CorrelationId = "abcd";
            message.ApplicationProperties.Add("SequenceId", 'a');

            await pipelineEventQueueSender.SendMessageAsync(message);
        }
    }
}
