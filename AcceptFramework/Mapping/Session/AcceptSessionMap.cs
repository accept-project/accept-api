using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Session;

namespace AcceptFramework.Mapping.Session
{
    public class AcceptSessionMap: ClassMap<AcceptSession>
    {

        public AcceptSessionMap()
        {

            Table("Session");

            Id(e => e.Id)
               .Column("ID");

            Map(e => e.SessionId)
                .Column("SessionId").Length(250);

            Map(e => e.SessionCodeId)
                .Column("SessionCodeId").Not.Nullable().Length(250);

            Map(e => e.ApiKey)
           .Column("ApiKey").Length(500);

            Map(e => e.OriginHost)
          .Column("OriginHost").Length(500);

            Map(e => e.OriginIp)
               .Column("OriginIp").Length(500);       


            Map(e => e.StartTime)
                .Column("StartTime");

            Map(e => e.EndTime)
                .Column("EndTime");

            Map(e => e.RequestedUrl)
              .Column("RequestedUrl").Length(500);


            Map(e => e.CachedValues)
                .Column("CachedValues").Length(500);

            Map(e => e.Context)
              .Column("Context").CustomSqlType("ntext").Length(1073741823);

        }
    }
}
