using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrchestrationFunctionApp.Functions
{
    public class PipelineDispatcher
    {
        [FunctionName("pipeline-dispatcher")]
        public void Run([TimerTrigger("0 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
