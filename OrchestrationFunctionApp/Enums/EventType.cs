using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Enums
{
    internal enum EventType
    {
        EmptyEvent = 1,
        InlineJson,
        JsonFile,
        FlatFile,
        XmlFile
    }
}
