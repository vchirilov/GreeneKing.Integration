using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Models
{
    public record MessageResponse
    {
        public IEnumerable<ServiceBusReceivedMessage> Messages { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Payload => Messages
            .OrderByDescending(x => x.SequenceNumber)
            .Select(x => x.Body.ToString())
            .FirstOrDefault();
    }
}
