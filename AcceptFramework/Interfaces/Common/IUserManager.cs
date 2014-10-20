using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Interfaces.Common
{
    public interface IUserManager
    {
        string RegisterUser(string userName, string password, string uiLanguage);
        
        User GetUser(string userName, string password);
        
        User GetUser(string confirmationCode);
        
        User GetUserByUserName(string userName);
        
        User GetUserBySecretKey(string userSecretKey);
        
        User GetUser(int Id);
        
        string RecoverUserPassword(string userName, out string languageUi);
        
        User UpdateUser(User user);
        
        User ChangeUserPassword(string userName, string password);
        
        List<User> GetAllPostEditUserInProject(Project p);
        
        List<User> GetAll();
        
        #region External Users
        ExternalPostEditUser GetExternalPostEditUserByUserName(string userName);
        ExternalPostEditUser GetExternalPostEditUserByUniqueName(string uniqueName);
        List<ExternalPostEditUser> GetAllExternalPostEditUserInProject(Project p);
        ExternalPostEditUser GetExternalUserByUserName(string userName);
        ExternalUserProjectRole AddExternalUserToExternalProject(Project p, string userName, int projectId, string userRoleinProject);
        #endregion

        void RemoveUserFromPostEditProject(Project p, User u, ExternalPostEditUser eu);

        ApiKeys CreateApiKey(string userName, string dnsName, string ipAdress, string appName, string organization, string description);
       
        bool DeleteApiKey(string userName, string apiKey);
       
        ApiKeys UpdateApiKey(string userName, string apiKey, string dnsName, string ipAddress, string appName, string organization, string description);

        ApiKeys GetApiKey(string apiKey);
        
        ICollection<ApiKeys> GetAllApiKey(string userName);

        ApiKeys GetKey(string userName, string apiKey);

        Domain.PostEdit.UserDomainRole AddUserToDomain(string userName, int domainId, string userRoleinDomain);

        UserProjectRole AddUserToProject(Project p, string userName, int projectId, string userRoleinProject);

        User AddRoleToUser(string userName, string uniqueRoleName);

        bool GenerateUserRoles();
      
    }
}
