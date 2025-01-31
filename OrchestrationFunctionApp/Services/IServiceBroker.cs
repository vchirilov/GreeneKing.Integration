using OrchestrationFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public interface IServiceBroker
    {
        Task PublishJmsQueueAllMessagesAsync<T>(T model);
        Task PublishJmsQueueOrchestrationsAsync<T>(T model);
        Task<MessageResponse> RetrieveAsync(string queue);
        Task<IList<string>> PeekExistingOrchestrationsAsync();
    }
}
