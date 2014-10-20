using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Common
{
    public class UniverseMap: ClassMap<Universe> 
    {
        public UniverseMap()
        {

            Table("Universe");

            Id(e => e.Id)
           .Column("ID");

            Map(e => e.Status)
                 .Column("Status");

            Map(e => e.UniverseName)
                  .Column("UniverseName").Length(250);

        }

    }
}
