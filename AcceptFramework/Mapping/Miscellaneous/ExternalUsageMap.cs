using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Miscellaneous;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Miscellaneous
{
    public class ExternalUsageMap: ClassMap<ExternalUsage>
    {
        public ExternalUsageMap()
        {
            Table("ExternalUsage");
            Id(e => e.Id);
            Map(e => e.InstanceIdentifier).Length(500);
            Map(e => e.UsageCount);
            Map(e => e.LastUpdate);
            Map(e => e.Metadata).Length(2500);;
            
        }
    
    }
}
