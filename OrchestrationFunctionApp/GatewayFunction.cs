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

            // Initialize Service bus connection 
            string connectionstring = Environment.GetEnvironmentVariable("QueueConnectionString");
            ServiceBusClient serviceBusClient = new ServiceBusClient(connectionstring);

            // Initialize a sender object with queue name
            var pipelineEventQueueSender = serviceBusClient.CreateSender("pipeline-event");

            // Create message for service bus
            dynamic payload = JsonConvert.DeserializeObject(requestBody);
            string sessionId = (string)payload.CorrelationId;
            //string sessionId = Guid.NewGuid().ToString();

            for (int i = 1; i <= 10; i++)
            {
                // Send the Message 
                ServiceBusMessage message = new ServiceBusMessage(requestBody);
                message.CorrelationId = sessionId;
                message.ApplicationProperties.Add("SequenceId", i);
                await pipelineEventQueueSender.SendMessageAsync(message);
            }

            string responseMessage = "Sent with success.";
            return new OkObjectResult(responseMessage);

        }
    }
}
