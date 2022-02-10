using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ScheduelAPISamples
{
    [DataContract]
    public class OperationSetResponse
    {
        [DataMember(Name = "operationSetId")]
        public Guid OperationSetId { get; set; }

        [DataMember(Name = "operationSetDetailId")]
        public Guid OperationSetDetailId { get; set; }

        [DataMember(Name = "operationType")]
        public string OperationType { get; set; }

        [DataMember(Name = "recordId")]
        public string RecordId { get; set; }

        [DataMember(Name = "correlationId")]
        public string CorrelationId { get; set; }
    }
}
