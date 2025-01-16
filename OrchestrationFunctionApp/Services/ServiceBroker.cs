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
    public class ServiceBroker: IServiceBroker
    {
        private readonly ILogger<ServiceBroker> _logger;
        private readonly ServiceBusSettings _serviceBusSettings;        
        private readonly IConfiguration _configuration;
        private readonly GreeeKingMessageBusContext _dbContext;
        private readonly int _delay;
        private IList<ServiceBusReceivedMessage> _messages = new List<ServiceBusReceivedMessage>();
        private IList<string> _exceptions = new List<string>();
        

        public ServiceBroker(ILogger<ServiceBroker> logger, IOptions<ServiceBusSettings> serviceBusSettings, IConfiguration configuration, GreeeKingMessageBusContext dbContext)
        {
            _logger = logger;
            _serviceBusSettings = serviceBusSettings.Value;
            _configuration = configuration;
            _dbContext = dbContext;

            _logger.LogWarning($"Queue defined in configuration is [{_serviceBusSettings.JmsQueueName}]");
            _delay = int.TryParse(_configuration[ConfigurationKeys.Pause], out int delay) ? delay : 3000;
        }

        public async Task PublishAsync<T>(T model)
        {
            var serviceBrokerClient = new ServiceBusClient(_serviceBusSettings.ConnectionString);
            var sender = serviceBrokerClient.CreateSender(_serviceBusSettings.JmsQueueName);

            await SendMessageAsync(sender, model);
        }

        public async Task<MessageResponse> RetrieveAsync(string queue)
        {
            var serviceBrokerClient = new ServiceBusClient(_serviceBusSettings.ConnectionString);
            var queueProcessor = serviceBrokerClient.CreateProcessor(queue, new ServiceBusProcessorOptions());

            try
            {
                queueProcessor.ProcessMessageAsync += MessageHandler;
                queueProcessor.ProcessErrorAsync += MessageErrorHandler;
                
                await queueProcessor.StartProcessingAsync();
                await Task.Delay(_delay);
                await queueProcessor.StopProcessingAsync();

                return new MessageResponse { Messages = _messages, Errors = _exceptions };                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return await Task.FromResult(new MessageResponse());
            }            
        }      

        public async Task SaveMessageAsync<T>(ServiceBusReceivedMessage message) where T: MsgBaseModel, new()
        {
            T baseModel = new();
            baseModel.SequenceNumber = message.SequenceNumber;
            baseModel.MessageId = message.MessageId;
            baseModel.EnqueuedTime = message.EnqueuedTime.UtcDateTime;

            var body = Encoding.UTF8.GetString(message.Body);
            var serviceBusMessageObject = JsonConvert.DeserializeObject<ServiceBusMessageObject>(body);

            baseModel.PipelineAction = serviceBusMessageObject.PipelineAction;
            baseModel.OrchestrationAction = serviceBusMessageObject.OrchestrationAction;
            baseModel.Payload = Convert.ToString(serviceBusMessageObject.Payload);
                        
            if (typeof(T) == typeof(MsgInlineJsonModel))
            {                
                var model = baseModel as MsgInlineJsonModel;
                var dbEntity = (MsgInlineJson)model;
                _dbContext.MsgInlineJsons.Add(dbEntity);                
            }            

            var affectedRows = await _dbContext.SaveChangesAsync();
            
            //Make sure that the record has been added to database table
            if (affectedRows > 0)
            {
                await PublishAsync(new MsgJmsModel { Action = baseModel.PipelineAction, MessageId = baseModel.MessageId });
            }
        }

        private async Task SendMessageAsync<T>(ServiceBusSender sender, T model)
        {
            var content = JsonConvert.SerializeObject(model);
            ServiceBusMessage message = new ServiceBusMessage(content);
            
            await sender.SendMessageAsync(message);
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            //var body = args.Message.Body.ToString();
            _messages.Add(args.Message);

            //Complete the message, message is deleted from the queue
            await args.CompleteMessageAsync(args.Message);
        }

        private async Task MessageErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception.Message);
            _exceptions.Add(args.Exception.Message);
            await Task.CompletedTask;
        }
    }
}
