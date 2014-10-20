using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Session;

namespace AcceptFramework.Interfaces.Session
{
    public interface ISessionManager
    {
         void InsertSession(AcceptSession acceptsession);

         AcceptSession GetNewAcceptSession();

         string[] GetSessionCachedValuesByCodeSessionId(string sessioncode);

         AcceptSession GetSessionByCodeSessionId(string sessioncode);

         ApiGlobalSession InsertGlobalSession(ApiGlobalSession globalSession);

         ApiGlobalSession GetGlobalSession(string globalSessionId);

         ApiGlobalSession UpdateGlobalSession(ApiGlobalSession globalSession);

         List<AcceptSession> GetChildSessionByGlobalSessionId(string globalSessionId);

         List<ApiGlobalSession> GetGlobalSessionRange(DateTime start, DateTime end);

         List<AcceptSession> GetChildSessionsByApiKeyDistinct(string apiKey);

         List<AcceptSession> GetChildSessionsByApiKeyDistinctRangedByDate(string apiKey, DateTime start, DateTime end);

    }
}
