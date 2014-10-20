using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class PhaseMap: ClassMap<Phase>
    {

        public PhaseMap()
        {
            Table("Phases");

            Id(e => e.Id);
            Map(e => e.PhaseName).Length(250);
            Map(e => e.ProcessName);
            Map(e => e.Date);
            Map(e => e.Tool);
            Map(e => e.ToolId).Length(2500);
            Map(e => e.JobId);
            Map(e => e.ContactEmail);

            
            HasManyToMany(e => e.Notes).Cascade.All().
            Not.LazyLoad().
            AsSet().
            Table("PhaseNotes").
            ParentKeyColumn("PhaseID").
            ChildKeyColumn("NoteID");

            References(e => e.PhaseCountGroup).Not.LazyLoad().Column("PhaseCountGroupId").Cascade.All();
        
        }
    
    }
}
