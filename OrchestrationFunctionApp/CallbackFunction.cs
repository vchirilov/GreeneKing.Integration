using System;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

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
                string messageBody = System.Text.Encoding.UTF8.GetString(message.Body);

                // Simulate processing
                log.LogInformation($"Message body: {messageBody}");
                await Task.Delay(5000);  // Simulate some processing work
            }
            catch (Exception ex)
            {
                log.LogError($"Failed to process message: {ex.Message}");
            }
        }
    }
}
