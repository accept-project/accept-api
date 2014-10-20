using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.PostEdit
{
    public class ExternalUserProjectRoleMap: ClassMap<ExternalUserProjectRole>
    {
        public ExternalUserProjectRoleMap()
        {
            Table("ExternalPostEditUserProjectRole");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.ExternalUser).Column("ExternalUserID").Not.LazyLoad();
            References(x => x.Role).Column("RoleID").Not.LazyLoad();
            References(x => x.Project).Column("ProjectID").Not.LazyLoad();
        
        }
    }
}
