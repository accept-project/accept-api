using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Miscellaneous
{
    public class ExternalUsage: DomainBase
    {
        public virtual string InstanceIdentifier { get; set; }
        public virtual int UsageCount { get; set; }
        public virtual DateTime? LastUpdate { get; set; }
        public virtual string Metadata { get; set; }

        public ExternalUsage()
        {
            this.InstanceIdentifier = string.Empty;
            this.UsageCount = 0;
            this.LastUpdate = null;
            this.Metadata = string.Empty;
        }

    }
}
