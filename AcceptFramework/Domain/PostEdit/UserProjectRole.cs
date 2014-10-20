using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Domain.PostEdit
{
    public class UserProjectRole: DomainBase
    {
    
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        public virtual Project Project { get; set; }

        public UserProjectRole() { }

        public UserProjectRole(User u, Role r, Project p) { User = u; Role = r; Project = p; }
    
    }
}
