using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class ThinkPhaseMap : ClassMap<ThinkPhase>
    {

        public ThinkPhaseMap()
        {
            Table("ThinkPhases");

            Id(e => e.Id);
            Map(e => e.PhaseName).Length(250);
            Map(e => e.ProcessName);
            Map(e => e.Date);
            Map(e => e.Tool);
            Map(e => e.ToolId).Length(250);
            Map(e => e.JobId);
            Map(e => e.ContactEmail);

            
            HasManyToMany(e => e.Notes).Cascade.All().
            Not.LazyLoad().
            AsSet().
            Table("ThinkPhaseNotes").
            ParentKeyColumn("ThinkPhaseID").
            ChildKeyColumn("NoteID");

            References(e => e.PhaseCountGroup).Not.LazyLoad().Column("PhaseCountGroupId").Cascade.All();
        
        }
    }
}
