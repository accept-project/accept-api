using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class PhaseCountGroupMap: ClassMap<PhaseCountGroup>
    {

        public PhaseCountGroupMap()
        {
            Table("CountGroups");

            Id(e => e.Id);
            Map(e => e.Name).Length(250);
            Map(e => e.DateAdded);

            HasManyToMany(e => e.PhaseCounts).Cascade.All().
            Not.LazyLoad().
            AsSet().
            Table("CountGroupsPhases").
            ParentKeyColumn("CountGroupID").
            ChildKeyColumn("PhaseID");
            
        
        }
      
    
    }
}
