using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class TranslationRevisionMap:ClassMap<TranslationRevision>
    {
        public TranslationRevisionMap()
        {

            Table("TranslationRevisions");

            Id(e => e.Id);
            Map(e => e.SegmentIndex);
            Map(e => e.PhaseName).Length(250);
            Map(e => e.State);

            Map(e => e.Source).CustomSqlType("ntext").Length(1073741823);
            Map(e => e.Target).CustomSqlType("ntext").Length(1073741823);

            Map(e => e.DateCreated);
            Map(e => e.LastUpdate);


            HasManyToMany(e => e.Phases).Cascade.All().
               Not.LazyLoad().
               AsSet().
               Table("TranslationRevisionPhase").
               ParentKeyColumn("TranslationRevisionID").
               ChildKeyColumn("PhaseID");


            HasManyToMany(e => e.AlternativeTranslations).Cascade.All().
             Not.LazyLoad().
             AsSet().
             Table("TranslationRevisionAlternativeTranslations").
             ParentKeyColumn("TranslationRevisionID").
             ChildKeyColumn("AlternativeTranslationID");



            HasManyToMany(e => e.ThinkPhases).Cascade.All().
              Not.LazyLoad().
              AsSet().
              Table("TranslationRevisionThinkPhase").
              ParentKeyColumn("TranslationRevisionID").
              ChildKeyColumn("ThinkPhaseID");

        
        }
    
    }
}
