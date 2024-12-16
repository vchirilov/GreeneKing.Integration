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
using OrchestrationFunctionApp.Services;
using System.Linq;
using Microsoft.Extensions.Configuration;
using OrchestrationFunctionApp.Persistence;

namespace OrchestrationFunctionApp.Functions
{
    public class QueuePayloadRetreiver
    {
        private readonly ILogger<QueuePayloadRetreiver> _logger;
        private readonly IServiceBroker _serviceBroker;
        private readonly IConfiguration _configuration;
        

        public QueuePayloadRetreiver(ILogger<QueuePayloadRetreiver> logger, IServiceBroker serviceBroker, IConfiguration configuration)
        {
            _logger = logger;
            _serviceBroker = serviceBroker;
            _configuration = configuration;           
        }

        [FunctionName("queue-payload-retreiver")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("[queue-payload-retreiver] azure-function has been activated.");
            
                var queue = await GetQueueName(req);
                
                var message = await _serviceBroker.RetrieveAsync(queue);

                if (message != null && message.Messages.Count() > 1)
                {
                    _logger.LogWarning($"Warning! {message.Messages.Count()} messages have been captured. Only one message must be captured.");
                }

                var defaultResponse = _configuration[ConfigurationKeys.DefaultResponse] ?? "0";
                                
                return new OkObjectResult(message.Payload ?? defaultResponse);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }            
        }

        private async Task<string> GetQueueName(HttpRequest req)
        {
            string queue = req.Query["queue"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            return queue ?? data?.queue; 
        }
    }
}
