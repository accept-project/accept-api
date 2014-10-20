using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.PostEdit
{
    public class UserProjectRoleMap: ClassMap<UserProjectRole>
    {

        public UserProjectRoleMap()
            {
                Table("UserProjectRole");
                LazyLoad();
                Id(x => x.Id).GeneratedBy.Identity().Column("ID");
                References(x => x.User).Column("UserID").Not.LazyLoad();
                References(x => x.Role).Column("RoleID").Not.LazyLoad();
                References(x => x.Project).Column("ProjectID").Not.LazyLoad();
            }
     
    
    }
}
