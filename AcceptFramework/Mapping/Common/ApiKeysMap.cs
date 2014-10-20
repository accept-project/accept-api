using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Mapping.Common
{
    public class ApiKeysMap: ClassMap<ApiKeys>
    {
        public ApiKeysMap()
        {
            Table("ApiKeys");

            Id(e => e.Id)
             .Column("ID");

            Map(e => e.ApiKey)
               .Column("ApiKey");

            Map(e => e.CreationDate)
                .Column("CreationDate");

            Map(e => e.KeyDns)
                   .Column("KeyDns");

            Map(e => e.KeyIp)
                .Column("KeyIp");

            Map(e => e.KeyStatus)
                    .Column("KeyStatus");

            Map(e => e.ApplicationName)
                   .Column("ApplicationName");

            Map(e => e.Organization)
                   .Column("Organization");

            Map(e => e.Description)
                   .Column("Description");

        }

    }
}
