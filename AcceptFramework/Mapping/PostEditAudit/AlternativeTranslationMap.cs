using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class AlternativeTranslationMap: ClassMap<AlternativeTranslation>
    {

        public AlternativeTranslationMap()
        {
            Table("AlternativeTranslations");

            Id(e => e.Id);
            Map(e => e.PhaseName).Length(250);        
            Map(e => e.Target).CustomSqlType("ntext").Length(1073741823);
        
        }
    }
}
