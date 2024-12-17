using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Models
{
    public class MsgBaseModel
    {
        public long? SequenceNumber { get; set; }

        public string MessageId { get; set; }

        public DateTime? EnqueuedTime { get; set; }

        public string Action { get; set; }

        public string Payload { get; set; }

        public bool? Processed { get; set; }
    }
}
