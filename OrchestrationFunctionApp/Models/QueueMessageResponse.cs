using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Models
{
    public record QueueMessageResponse
    {
        public IEnumerable<string> Messages { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Payload => Messages.FirstOrDefault();
    }
}
