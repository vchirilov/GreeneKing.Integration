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
        private readonly ServiceBusSettings _serviceBusSettings;        
        private readonly IConfiguration _configuration;
        private readonly IRepository _repository;
        private readonly int _delay;
        private IList<ServiceBusReceivedMessage> _messages = new List<ServiceBusReceivedMessage>();
        private IList<string> _exceptions = new List<string>();
        

        public EventProcessor(ILogger<EventProcessor> logger, IOptions<ServiceBusSettings> serviceBusSettings, IConfiguration configuration, IRepository repository)
        {
            _logger = logger;
            _serviceBusSettings = serviceBusSettings.Value;
            _configuration = configuration;
            _repository = repository;

            _logger.LogWarning($"Queue defined in configuration is [{_serviceBusSettings.JmsQueueName}]");
            _delay = int.TryParse(_configuration[ConfigurationKeys.Pause], out int delay) ? delay : 3000;
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
                _ => throw new NotSupportedException($"Unsupported model type: {typeof(T)}")
            };

            //// Publish message if database operation succeeded
            //if (affectedRows > 0)
            //{
            //    await PublishAsync(new MsgJmsModel
            //    {
            //        Action = model.PipelineAction,
            //        MessageId = model.MessageId
            //    });
            //}
        }        
    }
}
