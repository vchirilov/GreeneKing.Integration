using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Models
{
    public record MsgJmsModel
    {
        public string MessageId { get; set; }
        public string PipelineAction {  get; set; }
        public string OrchestrationAction { get; set; }        
    }
}

