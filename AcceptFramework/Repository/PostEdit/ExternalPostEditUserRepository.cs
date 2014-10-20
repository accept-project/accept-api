using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Repository.PostEdit
{
    internal static class ExternalPostEditUserRepository
    {

        public static IEnumerable<ExternalPostEditUser> GetAll()
        {
            return new RepositoryBase<ExternalPostEditUser>().Select();
        }

        public static ExternalPostEditUser Insert(ExternalPostEditUser record)
        {
          return  new RepositoryBase<ExternalPostEditUser>().Create(record);
        }

        public static void Delete(ExternalPostEditUser record)
        {
             new RepositoryBase<ExternalPostEditUser>().Delete(record);
        }

        public static ExternalPostEditUser GetExternalPostEditUser(int Id)
        {
            return new RepositoryBase<ExternalPostEditUser>().Select(a => a.Id == Id).FirstOrDefault();
        }

        public static ExternalPostEditUser GetExternalPostEditUser(string externalUserName)
        {
            return new RepositoryBase<ExternalPostEditUser>().Select(a => a.ExternalUserName == externalUserName).FirstOrDefault();
        }

        public static ExternalPostEditUser GetExternalPostEditUserByUniqueName(string uniqueUserName)
        {
            return new RepositoryBase<ExternalPostEditUser>().Select(a => a.UniqueIdentifier == uniqueUserName).FirstOrDefault();
        }

        
        public static ExternalPostEditUser GetExternalPostEditUserByUserName(string externalUserName)
        {
            return new RepositoryBase<ExternalPostEditUser>().Select(a => a.ExternalUserName == externalUserName).FirstOrDefault();
        }
      
        public static ExternalPostEditUser UpdateExternalPostEditUser(ExternalPostEditUser externalUser)
        {
            return new RepositoryBase<ExternalPostEditUser>().Update(externalUser);
        }

        public static IEnumerable<ExternalPostEditUser> GetAllByUserName(string externalUserName)
        {
            return new RepositoryBase<ExternalPostEditUser>().Select(a => a.ExternalUserName == externalUserName);
        }

    }
}
