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

namespace OrchestrationFunctionApp
{
    public static class GatewayFunction
    {
        [FunctionName("GatewayFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Gateway function has started...");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Initialize queue sender
            string connectionstring = Environment.GetEnvironmentVariable("QueueConnectionString");
            ServiceBusClient serviceBusClient = new ServiceBusClient(connectionstring);
            var pipelineEventQueueSender = serviceBusClient.CreateSender("pipeline-event");

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
