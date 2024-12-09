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

namespace OrchestrationFunctionApp.Functions
{
    public class QueuePayloadRetreiver
    {
        private readonly ILogger<QueuePayloadRetreiver> _logger;
        private readonly IServiceBroker _serviceBroker;

        public QueuePayloadRetreiver(ILogger<QueuePayloadRetreiver> logger, IServiceBroker serviceBroker)
        {
            _logger = logger;
            _serviceBroker = serviceBroker;
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

                var result = await _serviceBroker.RetrieveAsync();

                return new OkObjectResult("Hello World");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
            
        }
    }
}
