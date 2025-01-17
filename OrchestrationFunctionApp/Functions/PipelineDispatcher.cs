using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Services;

namespace OrchestrationFunctionApp.Functions
{
    public class PipelineDispatcher
    {
        private readonly ILogger<PipelineDispatcher> _logger;
        private readonly IRepository _repository;
        private readonly IServiceBroker _serviceBroker;

        public PipelineDispatcher(ILogger<PipelineDispatcher> logger, IRepository repository, IServiceBroker serviceBroker)
        {
            _logger = logger;
            _repository = repository;
            _serviceBroker = serviceBroker;
        }

        [FunctionName("pipeline-dispatcher")]
        public async Task Run([TimerTrigger("0 * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                _logger.LogInformation($"Function [pipeline-dispatcher] has started");

                var eligibleItems = await GetMsgJsmModelsAsync();

                _logger.LogInformation($"{eligibleItems.Count} of new events have been identified.");

                foreach (var item in eligibleItems)
                {
                    await _serviceBroker.PublishAsync(item);
                    await Task.Delay(200);
                }

                _logger.LogInformation($"Function [pipeline-dispatcher] has finished");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Function [pipeline-dispatcher] has failed with error message: {ex}");
            }
            
        }

        private async Task<IList<MsgJmsModel>> GetMsgJsmModelsAsync()
        {
            var eligibleEvents = (await _repository.GetEligibleEmptyEventItems())
                .Select(x => new MsgJmsModel { MessageId = x.MessageId, PipelineAction = x.PipelineAction, OrchestrationAction = x.OrchestrationAction }).ToList();

            eligibleEvents.AddRange((await _repository.GetEligibleInlineJsonItems())
                .Select(x => new MsgJmsModel { MessageId = x.MessageId, PipelineAction = x.PipelineAction, OrchestrationAction = x.OrchestrationAction }).ToList());

            eligibleEvents.AddRange((await _repository.GetEligibleJsonFileItems())
                .Select(x => new MsgJmsModel { MessageId = x.MessageId, PipelineAction = x.PipelineAction, OrchestrationAction = x.OrchestrationAction }).ToList());

            eligibleEvents.AddRange((await _repository.GetEligibleFlatFileItems())
                .Select(x => new MsgJmsModel { MessageId = x.MessageId, PipelineAction = x.PipelineAction, OrchestrationAction = x.OrchestrationAction }).ToList());

            return eligibleEvents;
        }
    }
}
