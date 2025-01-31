using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using OrchestrationFunctionApp.Enums;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Persistence.Entities;
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
        public async Task Run([TimerTrigger("* * * * *")] TimerInfo myTimer, ILogger log)
        {            
            try
            {
                _logger.LogInformation($"Function [pipeline-dispatcher] has started");

                await ProcessNewEmptyEventItems();
                await ProcessNewInlineJsonItems();
                await ProcessNewJsonFileItems();
                await ProcessNewFlatFileItems();                

                _logger.LogInformation($"Function [pipeline-dispatcher] has finished");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Function [pipeline-dispatcher] has failed with error message: {ex}");
            }

        }
        
        private async Task ProcessNewItems<TEntity>(Func<Task<IList<TEntity>>> func, EventType eventType)  where TEntity : class
        {
            //If pipeline dispatcher is disabled then, exit the method
            if (!await _repository.IsPipelineDispatcerEnabled((int)eventType))
            {
                return;
            }

            var items = (await func())
                .Select(entity =>
                new
                {
                    Id = (int)typeof(TEntity).GetProperty("Id")!.GetValue(entity)!,
                    model = new MsgJmsModel
                    {
                        MessageId = (string)typeof(TEntity).GetProperty("MessageId")!.GetValue(entity)!,
                        PipelineAction = (string)typeof(TEntity).GetProperty("PipelineAction")!.GetValue(entity)!,
                        OrchestrationAction = (string)typeof(TEntity).GetProperty("OrchestrationAction")!.GetValue(entity)!,
                        Payload = (string)typeof(TEntity).GetProperty("Payload")!.GetValue(entity)!
                    }
                }).ToList();

            foreach (var item in items)
            {
                try
                {
                    await _serviceBroker.PublishJmsQueueAllMessagesAsync(item);
                    await _repository.UpdateStatus<TEntity, int>(item.Id);                    
                    _logger.LogInformation($"Message {item.model.MessageId} has been published to queue [{Constants.QUEUE_JMS_JOB_ALL_MESSAGES}] and flagged with Processed = 1");
                    await Task.Delay(200);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Publishing {eventType} to queue [{Constants.QUEUE_JMS_JOB_ALL_MESSAGES}] has failed with exception: {ex.Message}", ex);
                }
            }

            await UpdateJmsQueueOrchestrations(items.Select(x => x.model.OrchestrationAction).ToList());
        }

        private async Task UpdateJmsQueueOrchestrations(IList<string> orchestratiosWithStatusZero)
        {
            var existingOrcestrationsInQueue = await _serviceBroker.PeekExistingOrchestrationsAsync();

            var cleanedStrings = existingOrcestrationsInQueue.Select(s => s.Trim('"', '\'')).ToList();

            var newOrchestrations = orchestratiosWithStatusZero.Except(cleanedStrings);

            foreach (var orchestration in newOrchestrations)
            {
                var cleanValue = orchestration.Trim('"', '\'');
                await _serviceBroker.PublishJmsQueueOrchestrationsAsync(orchestration);
            }
        }

        private async Task ProcessNewEmptyEventItems()
        {
            await ProcessNewItems<MsgEmptyEvent>(_repository.GetEligibleEmptyEventItems, EventType.EmptyEvent);
        }

        private async Task ProcessNewInlineJsonItems()
        {
            await ProcessNewItems<MsgInlineJson>(_repository.GetEligibleInlineJsonItems, EventType.InlineJson);
        }

        private async Task ProcessNewJsonFileItems()
        {
            await ProcessNewItems<MsgJsonFile>(_repository.GetEligibleJsonFileItems, EventType.JsonFile);
        }

        private async Task ProcessNewFlatFileItems()
        {
            await ProcessNewItems<MsgFlatFile>(_repository.GetEligibleFlatFileItems, EventType.FlatFile);
        }

    }
}