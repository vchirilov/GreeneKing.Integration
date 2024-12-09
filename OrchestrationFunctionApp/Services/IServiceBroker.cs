using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public interface IServiceBroker
    {
        Task PublishAsync(object message);
        Task<IList<string>> RetrieveAsync();
    }
}
