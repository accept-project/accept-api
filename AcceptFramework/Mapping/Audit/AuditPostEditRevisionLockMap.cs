using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Audit;

namespace AcceptFramework.Mapping.Audit
{
    public class AuditPostEditRevisionLockMap: ClassMap<AuditPostEditRevisionLock>
    {

        public AuditPostEditRevisionLockMap() {

            Table("AuditPostEditRevisionLock");

            Id(e => e.Id)
            .Column("ID");

            Map(e => e.UserName)
               .Column("UserName");

            Map(e => e.TextId)
             .Column("TextId");

            Map(e => e.UserAction)
                .Column("UserAction");

            Map(e => e.Meta)
              .Column("Meta");

            Map(e => e.ActionTimeStamp)
                .Column("ActionTimeStamp");

        
        }
    
    }

}
