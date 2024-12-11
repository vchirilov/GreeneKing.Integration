using Azure.Messaging.ServiceBus;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchestrationFunctionApp.Functions;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public class ServiceBroker: IServiceBroker
    {
        private readonly ILogger<ServiceBroker> _logger;
        private readonly ServiceBusSettings _serviceBusSettings;
        private IList<ServiceBusReceivedMessage> _messages = new List<ServiceBusReceivedMessage>();
        private IList<string> _exceptions = new List<string>();

        public ServiceBroker(ILogger<ServiceBroker> logger, IOptions<ServiceBusSettings> serviceBusSettings)
        {
            _logger = logger;
            _serviceBusSettings = serviceBusSettings.Value;

            _logger.LogInformation($"Queue defined in configuration is ${_serviceBusSettings.QueueName}");
        }

        public async Task PublishAsync(object message)
        {
            var serviceBrokerClient = new ServiceBusClient(_serviceBusSettings.ConnectionString);
            var queueSender = serviceBrokerClient.CreateSender(_serviceBusSettings.QueueName);

            await Task.CompletedTask;
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
                await Task.Delay(3000);
                await queueProcessor.StopProcessingAsync();

                return new MessageResponse { Messages = _messages, Errors = _exceptions };                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return await Task.FromResult(new MessageResponse());
            }            
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
