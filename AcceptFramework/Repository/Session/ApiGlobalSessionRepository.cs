using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Session;

namespace AcceptFramework.Repository.Session
{
    internal static class ApiGlobalSessionRepository
    {


        public static IEnumerable<ApiGlobalSession> GetAll()
        {
            return new RepositoryBase<ApiGlobalSession>().Select();
        }

        public static ApiGlobalSession Insert(ApiGlobalSession record)
        {
          return new RepositoryBase<ApiGlobalSession>().Create(record);
        }

        public static ApiGlobalSession Update(ApiGlobalSession record)
        {
            return new RepositoryBase<ApiGlobalSession>().Update(record);
        }

        public static ApiGlobalSession GetGlobalSessionByGlobalSessionId(string globalSessionId)
        {
            return new RepositoryBase<ApiGlobalSession>().Get(a => a.GlobalSessionId == globalSessionId);         
        }

        public static IEnumerable<ApiGlobalSession> GetAllRange(DateTime start, DateTime end)
        {
            return new RepositoryBase<ApiGlobalSession>().Select(a=> a.StartTime >= start && a.StartTime <= end);
        }
        
    
    }
}
