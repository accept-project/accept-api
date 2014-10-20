using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class TranslationUnitMap: ClassMap<TranslationUnit> 
    {
        public TranslationUnitMap()
        {

            Table("TranslationUnits");

            Id(e => e.Id);
            Map(e => e.SegmentIndex);
            Map(e => e.PhaseName).Length(250);
            Map(e => e.State);          
            Map(e => e.Source).CustomSqlType("ntext").Length(1073741823);
            Map(e => e.Target).CustomSqlType("ntext").Length(1073741823);

            HasManyToMany(e => e.AlternativeTranslations).Cascade.All().
               Not.LazyLoad().
               AsSet().
               Table("TranslationUnitAlternativeTranslations").
               ParentKeyColumn("TranslationUnitID").
               ChildKeyColumn("AlternativeTranslationID");
        }
    }
}
