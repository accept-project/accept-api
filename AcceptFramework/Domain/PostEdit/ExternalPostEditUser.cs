using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEdit
{
    public class ExternalPostEditUser: DomainBase
    {
        public virtual string ExternalUserName { get; set; }
        public virtual DateTime? CreationDate { get; set; }
        public virtual string UniqueIdentifier { get; set; }
        public virtual bool isDeleted { get; set; }

        public ExternalPostEditUser()
        {
            this.ExternalUserName = string.Empty;
            this.CreationDate = null;
            this.UniqueIdentifier = string.Empty;
            this.isDeleted = false;                
        }

        public ExternalPostEditUser(string externalUserName, DateTime creationDate,string uniqueIdentifier, bool isDeleted)
        {
            this.ExternalUserName = externalUserName;
            this.CreationDate = creationDate;
            this.UniqueIdentifier = uniqueIdentifier;
            this.isDeleted = isDeleted;
        }
    }

}
