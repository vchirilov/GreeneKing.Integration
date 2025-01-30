using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp
{
    public static class Constants
    {
        public const string QUEUE_JMS_JOB_ALL_MESSAGES = "sbq-event-all-messages-jms-job";
        public const string QUEUE_JMS_JOB_ORCHESTRATIONS = "sbq-event-orchestration-jms-job";
    }
}
