using Microsoft.Azure.Amqp.Framing;
using OrchestrationFunctionApp.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Models
{
    public  class MsgInlineJsonModel
    {
        public long? SequenceNumber { get; set; }

        public string MessageId { get; set; }

        public DateTime? EnqueuedTime { get; set; }

        public string Action { get; set; }

        public string Payload { get; set; }

        public bool? Processed { get; set; }

        public static explicit operator MsgInlineJson(MsgInlineJsonModel model)
        {
            return new MsgInlineJson
            {
                SequenceNumber = model.SequenceNumber,
                MessageId = model.MessageId,
                EnqueuedTime = model.EnqueuedTime,
                Action = model.Action,
                Payload = model.Payload,
                Processed = model.Processed
            };
        }
    }
}
