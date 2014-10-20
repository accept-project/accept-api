using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Domain.PostEdit
{
    public class UserDomainRole: DomainBase
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        public virtual AcceptFramework.Domain.Common.Domain Domain { get; set; }
        public UserDomainRole() { }
        public UserDomainRole(User u, Role r, AcceptFramework.Domain.Common.Domain d) { User = u; Role = r ; Domain = d; }
    }
}
