using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Session;
using AcceptFramework.Repository.Session;
using AcceptFramework.Interfaces.Session;

namespace AcceptFramework.Business.Session
{
    internal class SessionManager : ISessionManager
    {

        public void InsertSession(AcceptSession acceptsession)
        {
            AcceptSessionRepository.Insert(acceptsession);
        }

        public AcceptSession GetNewAcceptSession()
        {
            return new AcceptSession();        
        }

        public string[] GetSessionCachedValuesByCodeSessionId(string sessioncode)
        {           
            AcceptSession acceptsession = AcceptSessionRepository.GetCachedValuesBySessionCodeId(sessioncode);
            return acceptsession.CachedValues.Length > 0 ? acceptsession.CachedValues.Split(',') : new string[] { };
        }

        public AcceptSession GetSessionByCodeSessionId(string sessioncode)
        {            
            return AcceptSessionRepository.GetCachedValuesBySessionCodeId(sessioncode);           
        }

        public ApiGlobalSession InsertGlobalSession(ApiGlobalSession globalSession)
        {
            return ApiGlobalSessionRepository.Insert(globalSession);
        }

        public ApiGlobalSession UpdateGlobalSession(ApiGlobalSession globalSession)
        {
            return ApiGlobalSessionRepository.Update(globalSession);
        }

        public ApiGlobalSession GetGlobalSession(string  globalSessionId)
        {
            return ApiGlobalSessionRepository.GetGlobalSessionByGlobalSessionId(globalSessionId);
        }

        public List<ApiGlobalSession> GetGlobalSessionRange(DateTime start, DateTime end)
        {
            return ApiGlobalSessionRepository.GetAllRange(start,end).ToList();
        }

        public List<AcceptSession> GetChildSessionByGlobalSessionId(string globalSessionId)
        {
            return AcceptSessionRepository.GetChildSessionsByGlobalSessionId(globalSessionId).ToList();        
        }

        public List<AcceptSession> GetChildSessionsByApiKeyDistinct(string apiKey)
        {
            return AcceptSessionRepository.GetChildSessionsByApiKeyDistinct(apiKey).ToList();
        }

        public List<AcceptSession> GetChildSessionsByApiKeyDistinctRangedByDate(string apiKey, DateTime start, DateTime end)
        {
            return AcceptSessionRepository.GetChildSessionsByApiKeyDistinctRangedByDate(apiKey, start, end).ToList();
        }

        


    }
}
