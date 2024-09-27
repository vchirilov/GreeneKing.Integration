using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Options
{
    public class ServiceBusOptions
    {
        public string QueueConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}
