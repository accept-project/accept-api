using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Business.Utils;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Domain.Common
{
    public class User : DomainBase
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string UILanguage { get; set; }
        public virtual string ConfirmationCode { get; set; }
        public virtual int NativeLanguageID { get; set; }
        public virtual ICollection<Role> Roles { get; private set; }    
        public virtual string PasswordRecoveryCode { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual ICollection<ApiKeys> UserApiKeys { get; private set; }       
        public virtual ICollection<Project> UserProjects { get; private set; }
        public virtual ICollection<Role> UserRolesInProjects { get; private set; }
        public virtual ICollection<Domain> UserDomains { get; private set; }
        public virtual ICollection<Role> UserRolesInDomains { get; private set; }
        public virtual string SecretKeyCode { get; set; }
        public virtual DateTime? CreationDate { get; set; }

        public User()
        {
            this.Roles = new List<Role>();
            this.UserApiKeys = new List<ApiKeys>();
            this.UserProjects = new List<Project>();
            this.UserRolesInProjects = new List<Role>();
            this.UserDomains = new List<Domain>();
            this.UserRolesInDomains = new List<Role>();
            this.SecretKeyCode = string.Empty;
            this.CreationDate = null;
        }

        #region Properties


        public virtual void SetRole(Role role)
        {
            if (!Roles.Contains(role))
            {
                Roles.Clear();
                Roles.Add(role);
            }
        }


        public virtual void AddApiKey(ApiKeys key)
        {
            if (!UserApiKeys.Contains(key))
            {
                UserApiKeys.Add(key);
            }
        }

        public virtual void Confirm()
        {
            ConfirmationCode = string.Empty;
        }

        public override void Validate()
        {
            if (!StringUtils.EmailValidator(this.UserName))
                throw new ArgumentException("A valid e-mail address must be provided.", "UserName");
        }

        #endregion
    }
}
