using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Acrolinx
{
    [DataContract]
    [Serializable]
    public class AcrolinxResponseStatus
    {
        [DataMember(Name = "state", IsRequired = false)]
        public string State { get; set; }

        [DataMember(Name = "percentCurrentRunningPhase", IsRequired = false)]
        public string PercentCurrentRunningPhase { get; set; }    
    }
}