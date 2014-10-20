using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Interfaces.Common;
using AcceptFramework.Domain.Common;
using AcceptFramework.Repository.Common;
using AcceptFramework.Business.Utils;
using System.Net.Mail;
using AcceptFramework.Repository.PostEdit;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Business.Common
{
    internal class UserManager: IUserManager
    {
        public UserManager()
        { 
                
        }

        public bool GenerateUserRoles()
        {
            //TODO: create all user roles.
            if (RolesRepository.GetAll().Count() == 0)
            {
                Role guest = new Role("Guest User", "Guest");
                Role admnistrator = new Role("Portal Administrator", "Admin");
                Role projectAdmin = new Role("Project Administrator", "ProjAdmin");
                Role projectUser = new Role("Project User", "ProjUser");
                Role domainUser = new Role("Domain User", "DomainUser");
                Role domainAdmin = new Role("Domain Administrator", "DomainAdmin");
                Role projectCreation = new Role("Project Creator", "ProjectCreator");
                Role projectPartner = new Role("Project Partner", "ProjPartner");
                Role sigAdmin = new Role("SIG Administrator", "SIGAdmin");
                Role projectMantainer = new Role("Post Edit Project Maintainer", "PostEditProjMaintainer");               
                RolesRepository.Insert(guest);
                RolesRepository.Insert(admnistrator);
                RolesRepository.Insert(projectAdmin);
                RolesRepository.Insert(projectUser);
                RolesRepository.Insert(domainUser);
                RolesRepository.Insert(domainAdmin);
                RolesRepository.Insert(projectCreation);
                RolesRepository.Insert(projectPartner);
                RolesRepository.Insert(sigAdmin);
                RolesRepository.Insert(projectMantainer);

                return true;
            }

            return false;
        }

        public User ChangeUserPassword(string userName, string password)
        {
            User userToUpdate = UserRepository.GetUserByUserName(userName);
            if (userToUpdate != null)
            {
                userToUpdate.PasswordRecoveryCode = null;
                userToUpdate.Password = StringUtils.ToMD5(password);
                userToUpdate.Validate();
                return UserRepository.UpdateUser(userToUpdate);
            }
            else
                return null;
        }

        public string RecoverUserPassword(string userName, out string languageUi)
        {
            if (!StringUtils.EmailValidator(userName))
                throw new ArgumentException("A valid e-mail address must be provided.", "UserName");
            else
            {

                User userPasswordRecover = UserRepository.GetUserByUserName(userName);
                if (userPasswordRecover != null && userPasswordRecover.ConfirmationCode == null)
                {                                    
                    userPasswordRecover.PasswordRecoveryCode = StringUtils.Generate32CharactersStringifiedGuid();                    
                    UserRepository.UpdateUser(userPasswordRecover);
                    languageUi = userPasswordRecover.UILanguage;
                    return userPasswordRecover.PasswordRecoveryCode;
                }
                else
                {
                    languageUi = string.Empty;
                    return string.Empty;
                }
            }
        }

        public string RegisterUser(string userName, string password, string uiLanguage)
        {
            User newAcceptPortalUser = new User();
            newAcceptPortalUser.UserName = userName;
            newAcceptPortalUser.UILanguage = uiLanguage;
            newAcceptPortalUser.Password = StringUtils.ToMD5(password);            
            newAcceptPortalUser.Roles.Add(RolesRepository.GetRole("Guest"));
            newAcceptPortalUser.ConfirmationCode = StringUtils.Generate32CharactersStringifiedGuid();
            newAcceptPortalUser.SecretKeyCode = StringUtils.GenerateTinyHash(userName + password + DateTime.UtcNow.ToString());
            newAcceptPortalUser.Validate();
            CheckDuplicateUser(userName);
            newAcceptPortalUser.CreationDate = DateTime.UtcNow;
            UserRepository.Insert(newAcceptPortalUser);
            return newAcceptPortalUser.ConfirmationCode;
        }

        public User GetUser(string userName, string password)
        {
            return UserRepository.GetUser(userName, StringUtils.ToMD5(password));
        }

        public User GetUser(string confirmationCode)
        {
            return UserRepository.GetUserByRegistrationCode(confirmationCode);
        }

        public User GetUserByUserName(string userName)
        {
            return UserRepository.GetUserByUserName(userName);
        }

        public User GetUserBySecretKey(string userSecretKey)
        {
            return UserRepository.GetUserBySecretKey(userSecretKey);
        }

        public User GetUser(int Id)
        {
            User u = UserRepository.GetUser(Id);

            if (u == null)
                throw new ArgumentNullException("Id", "User Not Found");

            List<UserDomainRole> userDomainRoles = UserDomainRoleRepository.GetUserDomainRoleByUser(u).ToList<UserDomainRole>();
            foreach (UserDomainRole udr in userDomainRoles)
            {
                u.UserDomains.Add(udr.Domain);
                u.UserRolesInDomains.Add(udr.Role);
            }

            List<UserProjectRole> userProjectRoles = UserProjectRoleRepository.GetUserProjectRoleByUser(u).ToList<UserProjectRole>();
            foreach (UserProjectRole upr in userProjectRoles)
            {
                u.UserProjects.Add(upr.Project);
                u.UserRolesInProjects.Add(upr.Role);
            }

            return u;
        }
       
        public User UpdateUser(User user)
        {
            return UserRepository.UpdateUser(user);
        }

        public User AddRoleToUser(string userName, string uniqueRoleName)
        {
            User user = UserRepository.GetUserByUserName(userName);
            if (user == null)
                throw new ArgumentNullException("User", "User Not Found");

            Role role = RolesRepository.GetRole(uniqueRoleName);

            if (role == null)
                throw new ArgumentNullException("User", "User Role Found");

            user.Roles.Add(role);

            return UserRepository.UpdateUser(user);
        }

        public List<User> GetAll()
        {
            return UserRepository.GetAll().ToList();
        }

        public Domain.PostEdit.UserDomainRole AddUserToDomain(string userName, int domainId, string userRoleinDomain)
        {

            User u = UserRepository.GetUserByUserName(userName);
            
            if (u == null || u.IsDeleted)
                throw new ArgumentException("Invalid User", "userName");
            
            Domain.Common.Domain d = DomainRepository.GetDomain(domainId);
            
            if(d == null)
                throw new ArgumentException("Invalid Domain", "domainId");

            Role r = RolesRepository.GetRole(userRoleinDomain);

            if(r==null)
                throw new ArgumentException("Invalid Role", "userRoleinDomain");

            Domain.PostEdit.UserDomainRole userDomainRole = null;
            userDomainRole = UserDomainRoleRepository.GetUserDomainRoleByUserAndDomain(u, d);

            if (userDomainRole == null)
            {
                userDomainRole = new Domain.PostEdit.UserDomainRole(u, r, d);
                return UserDomainRoleRepository.Insert(userDomainRole);
            }
            else
            {
                userDomainRole.User = u;
                userDomainRole.Role = r;
                userDomainRole.Domain = d;
                return UserDomainRoleRepository.UpdateUserDomainRole(userDomainRole);
            }                      
        }

        public UserProjectRole AddUserToProject(Project p, string userName, int projectId, string userRoleinProject)
        {
            User u = UserRepository.GetUserByUserName(userName);
            if (u == null || u.IsDeleted)
                throw new ArgumentException("Invalid User", "userName");
         
            Role r = RolesRepository.GetRole(userRoleinProject);
            if (r == null)
                throw new ArgumentException("Invalid Role", "userRoleinProject");

            UserProjectRole userProjectRole = null;
            userProjectRole = UserProjectRoleRepository.GetUserProjectRoleByUserAndProject(u, p);
            if (userProjectRole == null)
            {
                userProjectRole = new UserProjectRole(u, r, p);
                return UserProjectRoleRepository.Insert(userProjectRole);
            }
            else
            {
                userProjectRole.User = u;
                userProjectRole.Role = r;
                userProjectRole.Project = p;
                return UserProjectRoleRepository.UpdateUserProjectRole(userProjectRole);
            }
        }
             
        public List<User> GetAllPostEditUserInProject(Project p)
        {
            try
            {
                return UserProjectRoleRepository.GetUserProjectRoleByProject(p).ToList().Select(x => x.User).ToList<User>();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
                
        public void RemoveUserFromPostEditProject(Project p, User u, ExternalPostEditUser eu)
        {
            try
            {
                if (p.External)
                {
                    ExternalUserProjectRole externalProjectUserRole = ExternalUserProjectRoleRepository.GetUserProjectRoleByExternalUserAndProject(eu, p);
                    if (externalProjectUserRole == null)
                        throw new Exception("User is not in project.");

                    //external users are added and as so deleted in a volatil manner.
                    ExternalUserProjectRoleRepository.Delete(externalProjectUserRole);
                    ExternalPostEditUserRepository.Delete(eu);
                   
                }
                else
                {
                    UserProjectRole userProjectRole = UserProjectRoleRepository.GetUserProjectRoleByUserAndProject(u, p);
                    if (userProjectRole == null)
                        throw new Exception("User is not in project.");
                    UserProjectRoleRepository.Delete(userProjectRole);
                }
            }
            catch (Exception e)
            {                
                throw(e);
            }                    
        }
  
        #region External Projects

        public ExternalUserProjectRole AddExternalUserToExternalProject(Project p, string userName, int projectId, string userRoleinProject)
        {
            string fullExternalUserName = userName;
            List<ExternalPostEditUser> usersWithSameExternalName = ExternalPostEditUserRepository.GetAllByUserName(fullExternalUserName).ToList();
            if (usersWithSameExternalName != null && usersWithSameExternalName.Count > 0)
            {
                foreach (ExternalPostEditUser u in usersWithSameExternalName)
                {
                    ExternalUserProjectRole upr = ExternalUserProjectRoleRepository.GetUserProjectRoleByExternalUserAndProject(u, p);
                    if (upr != null)
                        throw new Exception("Duplicated user names within the same project are not allowed.");
                }
            }

            ExternalPostEditUser newExternaluser = ExternalPostEditUserRepository.Insert(new ExternalPostEditUser(userName, DateTime.UtcNow, (userName + p.Id + DateTime.UtcNow).ToMD5(), false));
            Role r = RolesRepository.GetRole(userRoleinProject);
            if (r == null)
                throw new ArgumentException("Invalid External User Role", "userRoleinProject");

            ExternalUserProjectRole userProjectRole = new ExternalUserProjectRole(newExternaluser, r, p);
            return ExternalUserProjectRoleRepository.Insert(userProjectRole);
        }

        public ExternalPostEditUser GetExternalPostEditUserByUserName(string userName)
        {
            try
            {
                return ExternalPostEditUserRepository.GetExternalPostEditUserByUserName(userName);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public ExternalPostEditUser GetExternalPostEditUserByUniqueName(string uniqueName)
        {
            try
            {
                return ExternalPostEditUserRepository.GetExternalPostEditUserByUniqueName(uniqueName);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public List<ExternalPostEditUser> GetAllExternalPostEditUserInProject(Project p)
        {
            try
            {
                return ExternalUserProjectRoleRepository.GetExternalUserProjectRoleByProject(p).ToList().Select(x => x.ExternalUser).ToList<ExternalPostEditUser>();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public ExternalPostEditUser GetExternalUserByUserName(string userName)
        {
            return ExternalPostEditUserRepository.GetExternalPostEditUserByUserName(userName);
        }

        #endregion

        #region ApiKeys

        public ApiKeys CreateApiKey(string userName, string dnsName, string ipAdress, string appName, string organization, string description)
        {
            try
            {
                if (!StringUtils.EmailValidator(userName))
                    return null;

                User u = UserRepository.GetUserByUserName(userName);
                if (u != null)
                {                 
                    int count = 0;
                    for (int i = 0; i < u.UserApiKeys.Count; i++)
                        if (u.UserApiKeys.ToList<ApiKeys>()[i].KeyStatus > 0)
                            count++;

                    if (count > 4)
                       return null;

                    ApiKeys apiKey = new ApiKeys();
                    apiKey.ApiKey = StringUtils.Generate32CharactersStringifiedGuid();
                    apiKey.KeyDns = dnsName;
                    apiKey.KeyIp = ipAdress;
                    apiKey.KeyDns = dnsName;
                    apiKey.ApplicationName = appName;
                    apiKey.Organization = organization;
                    apiKey.Description = description;
                    apiKey.CreationDate = DateTime.UtcNow;

                    ApiKeyRepository.Insert(apiKey);
                    apiKey = ApiKeyRepository.GetKey(apiKey.ApiKey);
                    u.UserApiKeys.Add(apiKey);
                    UserRepository.UpdateUser(u);
                    
                    return apiKey;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception e) {

                return null;
            }
        
        }

        public bool DeleteApiKey(string userName, string apiKey)
        {
            try
            {
                if (!StringUtils.EmailValidator(userName))
                    return false;

                User u = UserRepository.GetUserByUserName(userName);
                if (u != null)
                {
                   
                    for (int i = 0; i < u.UserApiKeys.Count; i++)
                        if (u.UserApiKeys.ToList<ApiKeys>()[i].ApiKey == apiKey)
                        {                          
                            u.UserApiKeys.ToList<ApiKeys>()[i].KeyStatus = 0;
                            ApiKeyRepository.UpdateKey(u.UserApiKeys.ToList<ApiKeys>()[i]);                              
                            return true;
                        }                                                          
                }
               
                    return false;               
            }
            catch (Exception e)
            {

                return false;
            }
        
        }

        public ApiKeys UpdateApiKey(string userName, string apiKey, string dnsName, string ipAddress, string appName, string organization, string description)
        {
            try
            {
                if (!StringUtils.EmailValidator(userName))
                    return null;

                User u = UserRepository.GetUserByUserName(userName);
                if (u != null)
                {
                    for (int i = 0; i < u.UserApiKeys.Count; i++)
                        if (u.UserApiKeys.ToList<ApiKeys>()[i].ApiKey == apiKey)
                        {
                            ApiKeys key = this.GetApiKey(apiKey);                           
                            key.KeyDns = dnsName;
                            key.KeyIp = ipAddress;
                            key.ApplicationName = appName;
                            key.Organization = organization;
                            key.Description = description;
                            return ApiKeyRepository.UpdateKey(key);                                                        
                        }

                    return null;                                       
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }

        }

        public ApiKeys GetApiKey(string apiKey)
        {
            try
            {
                ApiKeys key = ApiKeyRepository.GetKey(apiKey);
                return key;
                              
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public ApiKeys GetKey(string userName, string apiKey)
        {

            try
            {
                if (!StringUtils.EmailValidator(userName))
                    return null;

                User u = UserRepository.GetUserByUserName(userName);
                if (u != null)
                {
                    foreach (ApiKeys key in u.UserApiKeys)
                    {
                        if (key.ApiKey == apiKey)
                            return key;
                    }
                }
              
                return null;                
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public ICollection<ApiKeys> GetAllApiKey(string userName)
        {

            try
            {
                if (!StringUtils.EmailValidator(userName))
                    return null;

                User u = UserRepository.GetUserByUserName(userName);
                if (u != null)
                {
                    return u.UserApiKeys;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        
        
        }

        #endregion

        #region Private

        private void CheckDuplicateUser(string userName)
        {
            if (UserRepository.GetUserByUserName(userName) != null)
                throw new ArgumentException("The e-mail address already exists in database.", "UserName");
        }

        private void InsertUser(User user)
        {
            UserRepository.Insert(user);
        }

        #endregion
      
    }
}
