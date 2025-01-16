using Azure.Messaging.ServiceBus;
using OrchestrationFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public interface IEventProcessor
    {
        Task PublishAsync<T>(T model);
        Task<MessageResponse> RetrieveAsync(string queue);
        Task SaveMessageAsync<T>(ServiceBusReceivedMessage message) where T : MsgBaseModel, new();
    }
}
