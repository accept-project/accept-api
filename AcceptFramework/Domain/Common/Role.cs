using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Common
{
    public class Role: DomainBase
    {
        public virtual string RoleName { get; set; }
        public virtual string UniqueName { get; set; }

        public Role() { RoleName = string.Empty; UniqueName = string.Empty; }
        public Role(string roleName, string uniqueRoleName) { RoleName = roleName; UniqueName = uniqueRoleName; }

    }
}
