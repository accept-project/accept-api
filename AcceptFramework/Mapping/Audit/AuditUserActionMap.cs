using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Audit;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Audit
{
    public class AuditUserActionMap : ClassMap<AuditUserAction>
    {
        public AuditUserActionMap()
        {
            Table("AuditUserAction");

            Id(e => e.Id)
               .Column("ID");

            Map(e => e.SessionCodeId)
                .Column("SessionCodeId");

            Map(e => e.AuditTypeId)
                .Column("AuditTypeId");

            Map(e => e.RuleName)
                .Column("RuleName").Length(250);

            Map(e => e.AuditContext)
                .Column("AuditContext").CustomSqlType("ntext").Length(1073741823);

            Map(e => e.TextBefore)
              .Column("TextBefore").CustomSqlType("ntext").Length(1073741823);


            Map(e => e.TextAfter)
                .Column("TextAfter").CustomSqlType("ntext").Length(1073741823);


            Map(e => e.StartTime)
                .Column("StartTime");


            Map(e => e.EndTime)
                .Column("EndTime");

        
        }
    
    
    }
}
