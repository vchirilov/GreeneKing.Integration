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

        private async Task ProcessNewItems<TEntity>(Func<Task<IList<TEntity>>> func, string logPrefix)  where TEntity : class
        {
            var items = (await func())
                .Select(x =>
                new
                {
                    Id = (int)typeof(TEntity).GetProperty("Id")!.GetValue(x)!,
                    model = new MsgJmsModel
                    {
                        MessageId = (string)typeof(TEntity).GetProperty("MessageId")!.GetValue(x)!,
                        PipelineAction = (string)typeof(TEntity).GetProperty("PipelineAction")!.GetValue(x)!,
                        OrchestrationAction = (string)typeof(TEntity).GetProperty("OrchestrationAction")!.GetValue(x)!
                    }
                }).ToList();

            foreach (var item in items)
            {
                try
                {
                    await _serviceBroker.PublishAsync(item);
                    await _repository.UpdateStatus<TEntity, int>(item.Id);
                    _logger.LogInformation($"Message {item.model.MessageId} has been published to queue [sbq-event-jms-job] and flagged with Processed = 1");
                    await Task.Delay(200);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Publishing {logPrefix} to queue [sbq-event-jms-job] has failed with exception: {ex.Message}", ex);
                }
            }
        }

        private async Task ProcessNewEmptyEventItems()
        {
            await ProcessNewItems<MsgEmptyEvent>(_repository.GetEligibleEmptyEventItems, "EmptyEvent");
        }

        private async Task ProcessNewInlineJsonItems()
        {
            await ProcessNewItems<MsgInlineJson>(_repository.GetEligibleInlineJsonItems, "InlineJson");
        }

        private async Task ProcessNewJsonFileItems()
        {
            await ProcessNewItems<MsgJsonFile>(_repository.GetEligibleJsonFileItems, "JsonFile");
        }

        private async Task ProcessNewFlatFileItems()
        {
            await ProcessNewItems<MsgFlatFile>(_repository.GetEligibleFlatFileItems, "FlatFile");
        }

    }
}