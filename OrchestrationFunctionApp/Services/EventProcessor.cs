using Azure.Messaging.ServiceBus;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrchestrationFunctionApp.Functions;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Options;
using OrchestrationFunctionApp.Persistence;
using OrchestrationFunctionApp.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public class EventProcessor: IEventProcessor
    {
        private readonly ILogger<EventProcessor> _logger;
        private readonly IRepository _repository;        

        public EventProcessor(ILogger<EventProcessor> logger, IOptions<ServiceBusSettings> serviceBusSettings, IConfiguration configuration, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task SaveMessageAsync<T>(ServiceBusReceivedMessage message) where T : MsgBaseModel, new()
        {
            // Initialize the model and populate common properties
            var model = new T
            {
                SequenceNumber = message.SequenceNumber,
                MessageId = message.MessageId,
                EnqueuedTime = message.EnqueuedTime.UtcDateTime
            };

            // Deserialize message body
            var body = Encoding.UTF8.GetString(message.Body);
            var serviceBusMessageObject = JsonConvert.DeserializeObject<ServiceBusMessageObject>(body);

            model.PipelineAction = serviceBusMessageObject.PipelineAction;
            model.OrchestrationAction = serviceBusMessageObject.OrchestrationAction;
            model.Payload = Convert.ToString(serviceBusMessageObject.Payload);

            // Save the model and handle specific types
            var affectedRows = model switch
            {
                MsgInlineJsonModel inlineJsonModel => await _repository.SaveInlineJsonEvent((MsgInlineJson)inlineJsonModel),
                MsgEmptyEventModel emptyEventModel => await _repository.SaveEmptyEvent((MsgEmptyEvent)emptyEventModel),
                MsgJsonFileModel jsonFileModel => await _repository.SaveJsonFileEvent((MsgJsonFile)jsonFileModel),
                MsgFlatFileModel flatFileModel => await _repository.SaveFlatFileEvent((MsgFlatFile)flatFileModel),
                _ => throw new NotSupportedException($"Unsupported model type: {typeof(T)}")
            };
        }        
    }
}
