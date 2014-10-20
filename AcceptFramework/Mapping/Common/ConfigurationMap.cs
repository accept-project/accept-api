using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Common
{
    public class ConfigurationMap : ClassMap<Configuration>
    {
        public ConfigurationMap()
        {
            Table("Configuration");

            Id(e => e.Id)
           .Column("ID");

            Map(e => e.Address)
              .Column("Address").Length(500);

            Map(e => e.CheckType)
               .Column("CheckType").Length(500);

            Map(e => e.Context)
                 .Column("Context").Length(500);         


          References(e => e.Engine).Not.LazyLoad().Column("EngineId");


        }        
    }
}
