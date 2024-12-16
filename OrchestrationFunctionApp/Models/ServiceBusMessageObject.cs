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
        public string Action { get; set; }
        public string Platform { get; set; }
        [JsonProperty("content-type")]
        public string Contenttype { get; set; }
        public dynamic Payload { get; set; }
    }
}
