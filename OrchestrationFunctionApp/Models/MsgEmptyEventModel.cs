using OrchestrationFunctionApp.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Models
{
    public class MsgEmptyEventModel : MsgBaseModel
    {
        public static explicit operator MsgEmptyEvent(MsgEmptyEventModel model)
        {
            return new MsgEmptyEvent
            {
                SequenceNumber = model.SequenceNumber,
                MessageId = model.MessageId,
                EnqueuedTime = model.EnqueuedTime,
                PipelineAction = model.PipelineAction,
                OrchestrationAction = model.OrchestrationAction,
                //Payload = model.Payload,
                Processed = model.Processed
            };
        }
    }
}
