using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Common
{
    public class RoleMap: ClassMap<AcceptFramework.Domain.Common.Role>
    {

        public RoleMap()
        {
            Table("Role");

            Id(e => e.Id)
             .Column("ID");

            Map(e => e.RoleName)
                .Column("RoleName")
                .Not.Nullable();

            Map(e => e.UniqueName)
                  .Column("UniqueName")
                  .Not.Nullable();
        }
    
    }
}
