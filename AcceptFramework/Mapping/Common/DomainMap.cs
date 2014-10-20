using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Mapping.Common
{
    public class DomainMap : ClassMap<AcceptFramework.Domain.Common.Domain>
    {

        public DomainMap()
        {
            Table("Domain");

            Id(e => e.Id)
               .Column("ID");           

            Map(e => e.DomainName)
                .Column("DomainName").Length(250);

            Map(e => e.Status)
              .Column("Status");


            References(e => e.DomainUniverse).Not.LazyLoad().Column("UniverseId");

            HasManyToMany(x => x.Languages)
              .Table("DomainLanguage")
              .ParentKeyColumn("DomainId")
              .ChildKeyColumn("LanguageId").Not.LazyLoad()
              .Cascade.SaveUpdate();


        }

    }
}
