using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Repository.Common
{
    internal static class ApiKeyRepository
    {

        public static IEnumerable<ApiKeys> GetAll()
        {
            return new RepositoryBase<ApiKeys>().Select();
        }

        public static void Insert(ApiKeys record)
        {
            new RepositoryBase<ApiKeys>().Create(record);
        }

        public static ApiKeys GetKey(string apiKey)
        {
            return new RepositoryBase<ApiKeys>().Select(a => a.ApiKey == apiKey).FirstOrDefault();
        }

        public static ApiKeys UpdateKey(ApiKeys key)
        {
            return new RepositoryBase<ApiKeys>().Update(key);
        }

        public static ApiKeys GetKeyByStatus(int status)
        {
            return new RepositoryBase<ApiKeys>().Select(a => a.KeyStatus == status).FirstOrDefault();
        }


    }
}
