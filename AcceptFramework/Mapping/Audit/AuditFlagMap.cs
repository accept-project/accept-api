using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Audit;

namespace AcceptFramework.Mapping.Audit
{
    public class AuditFlagMap: ClassMap<AuditFlag>
    {
        public AuditFlagMap()
        {
            Table("AuditFlag");

            Id(e => e.Id)
              .Column("ID");

            Map(e => e.Flag)
                .Column("Flag");

            Map(e => e.Action)
                .Column("Action");

            Map(e => e.ActionValue)
                .Column("ActionValue");

            Map(e => e.SessionCodeId)
                .Column("SessionCodeId");

            Map(e => e.Ignored)
                .Column("Ignored");

            Map(e => e.Name)
                .Column("Name");

            Map(e => e.TextBefore)
              .Column("TextBefore").CustomSqlType("ntext").Length(1073741823);
            
            Map(e => e.TextAfter)
              .Column("TextAfter").CustomSqlType("ntext").Length(1073741823);

            Map(e => e.TimeStamp)
               .Column("TimeStamp");

            Map(e => e.RawValue)
               .Column("RawVal").CustomSqlType("ntext").Length(1073741823);

            Map(e => e.PrivateId)
               .Column("PrivateId").Length(250);

        }    
    }
}
