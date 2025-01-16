using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Models
{    public class ServiceBusMessageObject
    {
        public string Target { get; set; }
        
        [JsonProperty("pipeline_action")]
        public string PipelineAction { get; set; }
        
        [JsonProperty("orchestration_action")]
        public string OrchestrationAction { get; set; }
        
        [JsonProperty("content-type")]
        public string Contenttype { get; set; }
        
        public dynamic Payload { get; set; }
    }
}
