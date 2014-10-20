using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Audit;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Audit
{
    public class PageVisitAuditMap: ClassMap<PageVisitAudit>
    {
        public PageVisitAuditMap()
        {
            Table("AuditPageVisit");
            Id(e => e.Id)
            .Column("ID");
            Map(e => e.UserName).Length(320);
            Map(e => e.Description)
              .Column("Description").CustomSqlType("ntext").Length(1073741823);
            Map(x => x.Type).CustomType<int>();
            Map(x => x.Action).CustomType<int>();
            Map(e => e.TimeStamp);
            Map(e => e.Origin).Column("Origin").Length(320);
            Map(e => e.Meta).Column("Meta").CustomSqlType("ntext").Length(1073741823);
            Map(e => e.Language).Column("Language").Length(320);
            Map(e => e.UserAgent).Column("UserAgent").Length(320);
        }
    
    }
}
