using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrchestrationFunctionApp.Models;
using OrchestrationFunctionApp.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public class ServiceBroker : IServiceBroker
    {
        private readonly ILogger<ServiceBroker> _logger;
        private readonly ServiceBusSettings _serviceBusSettings;
        private readonly ServiceBusSender _serviceBusSender;

        private IList<ServiceBusReceivedMessage> _messages = new List<ServiceBusReceivedMessage>();
        private IList<string> _exceptions = new List<string>();


        public ServiceBroker(ILogger<ServiceBroker> logger, IOptions<ServiceBusSettings> serviceBusSettings)
        {
            _logger = logger;
            _serviceBusSettings = serviceBusSettings.Value;
            var serviceBrokerClient = new ServiceBusClient(_serviceBusSettings.ConnectionString);
            _serviceBusSender = serviceBrokerClient.CreateSender(_serviceBusSettings.JmsQueueName);
        }

        public async Task PublishAsync<T>(T model)
        {
            await SendMessageAsync(_serviceBusSender, model);
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
