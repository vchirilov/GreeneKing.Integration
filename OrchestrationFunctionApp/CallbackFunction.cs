using System;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrchestrationFunctionApp.Models;

namespace OrchestrationFunctionApp
{
    public class CallbackFunction
    {
        [FunctionName("CallbackFunction")]
        public static async Task Run(
        [ServiceBusTrigger("pipeline-event", Connection = "QueueConnectionString")] ServiceBusReceivedMessage message, ILogger log)
        {
            try
            {
                log.LogInformation($"Received message with SequenceId: {message.ApplicationProperties["SequenceId"]}");
                string requestBody = System.Text.Encoding.UTF8.GetString(message.Body);
                Workflow workflow = JsonConvert.DeserializeObject<Workflow>(requestBody);

                // Simulate processing
                log.LogInformation($"Message body: {JsonConvert.SerializeObject(workflow)}");
                await Task.Delay(5000);  // Simulate some processing work
            }
            catch (Exception ex)
            {
                log.LogError($"Failed to process message: {ex.Message}");
            }
        }
    }
}
