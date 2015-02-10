using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Mapping.Common
{
    public class UserMap : ClassMap<User>
    {
         public UserMap()
        {
            Table("Users");

            Id(e => e.Id);
            Map(e => e.UserName).Length(320);
            Map(e => e.Password).Length(250);
            Map(e => e.UILanguage).Length(5);
            Map(e => e.NativeLanguageID);
            Map(e => e.ConfirmationCode).Length(250);
            Map(e => e.PasswordRecoveryCode);
            Map(e => e.IsDeleted);
            Map(e => e.SecretKeyCode).Length(250);
            Map(e => e.CreationDate);

            HasManyToMany(e => e.Roles).
                Not.LazyLoad().
                AsSet().
                Table("UserRoles").
                ParentKeyColumn("UserID").
                ChildKeyColumn("RoleID");

            HasManyToMany(e => e.UserApiKeys).
              Not.LazyLoad().
              AsSet().
              Table("UserApiKeys").
              ParentKeyColumn("UserID").
              ChildKeyColumn("ApiKeyID");
           
        }
    }
}
