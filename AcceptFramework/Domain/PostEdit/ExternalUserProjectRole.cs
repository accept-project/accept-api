using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Domain.PostEdit
{
    public class ExternalUserProjectRole: DomainBase
    {
        public virtual ExternalPostEditUser ExternalUser { get; set; }
        public virtual Role Role { get; set; }
        public virtual Project Project { get; set; }
        public ExternalUserProjectRole() { }
        public ExternalUserProjectRole(ExternalPostEditUser u, Role r, Project p) { ExternalUser = u; Role = r; Project = p; }        
    }
}
