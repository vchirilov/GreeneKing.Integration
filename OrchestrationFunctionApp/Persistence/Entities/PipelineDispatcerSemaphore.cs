using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Persistence.Entities
{
    public class PipelineDispatcerSemaphore
    {
        public int Id { get; set; }
                
        public string QueueName { get; set; }
        
        public bool IsPipelineDispatcerEnabled { get; set; }
    }
}
