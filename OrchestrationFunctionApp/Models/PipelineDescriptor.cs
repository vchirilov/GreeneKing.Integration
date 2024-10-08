using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Models
{
    public class PipelineDescriptor
    {
        public Workflow[] Pipeline { get; set; }
    }

    public class Workflow
    {
        public int OrderId { get; set; }
        public string Service { get; set; }
        public string ServiceURL { get; set; }
        public string Method { get; set; }
        public dynamic Body { get; set; }
        public string Action { get; set; }
        public Parameters Parameters { get; set; }
    }

    public class Parameters
    {
        public string Argument1 { get; set; }
        public string Argument2 { get; set; }
        
    }    
}
