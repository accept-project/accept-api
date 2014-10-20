using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class AlternativeTranslation: DomainBase
    {
        public virtual string PhaseName { get; set; }
        public virtual string Target { get; set; }

        public AlternativeTranslation()
        { 
        
        }

        public AlternativeTranslation(string phaseName, string target)
        {
            this.PhaseName = phaseName;
            this.Target = target;
        }

    }
}
