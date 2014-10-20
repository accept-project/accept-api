using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;
using FluentNHibernate.Mapping;


namespace AcceptFramework.Mapping.PostEdit
{
    public class UserDomainRoleMap : ClassMap<UserDomainRole>
    {

        public UserDomainRoleMap()
        {
            Table("UserDomainRole");            
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.User).Column("UserID").Not.LazyLoad();
            References(x => x.Role).Column("RoleID").Not.LazyLoad();
            References(x => x.Domain).Column("DomainID").Not.LazyLoad();
        }
    }
}
