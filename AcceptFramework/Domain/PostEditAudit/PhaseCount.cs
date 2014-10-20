using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class PhaseCount:DomainBase
    {
        public virtual string PhaseName {get; set;}
        public virtual string CountType {get; set;}
        public virtual string Unit {get; set;}
        public virtual string Value {get; set;}

        public PhaseCount()
        {
            PhaseName = string.Empty;
            CountType = string.Empty;
            Unit = string.Empty;
            Value = string.Empty;
        
        }
    
    }
}
