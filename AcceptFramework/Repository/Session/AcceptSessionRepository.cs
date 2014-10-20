using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Session;

namespace AcceptFramework.Repository.Session
{
    internal static class AcceptSessionRepository
    {

        public static IEnumerable<AcceptSession> GetAll()
        {
            return new RepositoryBase<AcceptSession>().Select();
        }

        public static void Insert(AcceptSession record)
        {
            new RepositoryBase<AcceptSession>().Create(record);
        }

        public static AcceptSession GetCachedValuesBySessionCodeId(string sessionCodeId)
        {
            return new RepositoryBase<AcceptSession>().Get(a => a.SessionCodeId == sessionCodeId);
        }


        public static IEnumerable<AcceptSession> GetChildSessionsByGlobalSessionId(string globalSessionId)
        {
            return new RepositoryBase<AcceptSession>().Select(a => a.SessionId == globalSessionId);        
        }


        public static IEnumerable<AcceptSession> GetChildSessionsByApiKeyDistinct(string apiKey)
        {           
            return new RepositoryBase<AcceptSession>().Select(a => a.ApiKey == apiKey).Distinct(new GlobalSessionComparer());
        }

        public static IEnumerable<AcceptSession> GetChildSessionsByApiKeyDistinctRangedByDate(string apiKey, DateTime start, DateTime end)
        { 
            return new RepositoryBase<AcceptSession>().Select(a => a.ApiKey == apiKey && a.StartTime >= start && a.EndTime <= end).Distinct(new GlobalSessionComparer());
        }                
    
    }

    internal class GlobalSessionComparer : IEqualityComparer<AcceptSession>
    {
        #region IEqualityComparer<Contact> Members

        public bool Equals(AcceptSession x, AcceptSession y)
        {
            return x.SessionId.Equals(y.SessionId);
        }

        public int GetHashCode(AcceptSession obj)
        {
            return obj.SessionId.GetHashCode();
        }

        #endregion
    }

}
