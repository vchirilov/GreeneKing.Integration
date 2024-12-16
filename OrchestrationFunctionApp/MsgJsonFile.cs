using System;
using System.Collections.Generic;

namespace OrchestrationFunctionApp;

public partial class MsgJsonFile
{
    public int Id { get; set; }

    public long? SequenceNumber { get; set; }

    public string MessageId { get; set; }

    public DateTime? EnqueuedTime { get; set; }

    public string Action { get; set; }

    public string Payload { get; set; }

    public bool? Processed { get; set; }
}
