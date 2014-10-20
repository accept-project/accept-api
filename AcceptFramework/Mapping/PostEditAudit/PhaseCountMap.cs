using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class PhaseCountMap: ClassMap<PhaseCount>
    {
        public PhaseCountMap()
        {
            Table("PhaseCounts");

            Id(e => e.Id);
            Map(e => e.PhaseName).Length(250);
            Map(e => e.CountType);
            Map(e => e.Unit);
            Map(e => e.Value);
            
        
        
        }
    
    }
}
