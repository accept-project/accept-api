using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Mapping.Common
{
    public class LanguageMap: ClassMap<Language>
    {
        public LanguageMap()
        {
            Table("Language");

            Id(e => e.Id)
             .Column("ID");

            
            Map(e => e.LanguageName)
                  .Column("LanguageName").Length(250);

            Map(e => e.LanguageCode)
                      .Column("LanguageCode").Length(50);

            References(e => e.Configuration).Not.LazyLoad().Column("ConfigurationId");

        }
    }
}
