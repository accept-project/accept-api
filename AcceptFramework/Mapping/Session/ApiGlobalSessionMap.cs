using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Session;

namespace AcceptFramework.Mapping.Session
{
    public class ApiGlobalSessionMap: ClassMap<ApiGlobalSession>
    {

        public ApiGlobalSessionMap()
        {

            Table("ApiGlobalSession");
            Id(e => e.Id)
             .Column("ID");

            Map(e => e.GlobalSessionId)
              .Column("GlobalSessionId").Length(250);

            Map(e => e.SessionId)
                .Column("SessionId").Length(250);
          
            Map(e => e.StartTime)
           .Column("StartTime");

            Map(e => e.EndTime)
           .Column("EndTime");

            Map(e => e.Meta)
            .Column("Meta").Length(2500);

            Map(e => e.FinalContext)
            .Column("FinalContext").CustomSqlType("ntext").Length(1073741823);

           
        }
    
    
    }
}
