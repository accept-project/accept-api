using System.Collections.Generic;
using System.Linq;
using System.Text;

using AcceptApi.Areas.Api.Models.Core;
using AcceptApi.Areas.Api.Models.Authentication;
using System;

namespace AcceptApi.Areas.Api.Models
{
    public interface IAcceptApiManager
    {
        #region Core
        
        CoreApiResponse GenericRequest(Dictionary<string, string> parameters);
        CoreApiResponse GetResponse(Dictionary<string, string> inputparameters);
        CoreApiResponse GetResponseStatus(Dictionary<string, string> inputparameters);
        CoreApiResponse GetRuleSetNames(string language);
        CoreApiResponse GenericRealTimeRequest(Dictionary<string, string> parameters, string clientId, string requestedUrl, string index);
        CoreApiResponse GetRealTimeResponseStatus(Dictionary<string, string> parameters);
        CoreApiResponse GetRealTimeResponse(Dictionary<string, string> parameters);
        
        #endregion

        #region Grammar

        string GrammarLanguages();
        CoreApiResponse GrammarRequest(Dictionary<string, string> inputparams);

        #endregion

        #region Spelling
        
        CoreApiResponse SpellCheckRequest(Dictionary<string, string> inputparams);
        string SpellCheckLanguages();
        
        #endregion

        #region Style

        CoreApiResponse StyleCheckRequest(Dictionary<string, string> inputparams);
        string StyleCheckLanguages();
        
        #endregion

        #region Audit

        CoreApiResponse AuditUserAction(CoreApiAudit audit);

        CoreApiResponse AddAuditFlag(string flag, string action, string actionValue, string globalSessionId, string ignored, string name, string textBefore, string textAfter, DateTime timeStamp, string rawValue, string privateFlagId);

        CoreApiResponse AuditFinalContext(string globalSessionId, string textContent, DateTime timeStamp);

        CoreApiResponse GlobalSession(string globalSessionId);

        CoreApiResponse GlobalSessionRange(DateTime start, DateTime end);

        CoreApiResponse GlobalSessionApiKey(string apiKey, DateTime? start, DateTime? end, string userSecretKey);
        
        CoreApiResponse SimpleGlobalSessionRange(DateTime start, DateTime end);
        
        CoreApiResponse SimpleGlobalSessionApiKey(string apiKey, DateTime? start, DateTime? end, string userSecretKey);

        string SimpleGlobalSessionApiKeyCustom(string apiKey, DateTime? start, DateTime? end, string userSecretKey, string format);


        #endregion

        #region Authentication
        CoreApiResponse UpdateUserKey(ApiKeyRequestObject requestObject);
        CoreApiResponse DeleteUserKey(ApiKeyRequestObject requestObject);
        CoreApiResponse GetUserKeys(ApiKeyRequestObject requestObject);
        CoreApiResponse CreateUserKey(ApiKeyRequestObject requestObject);
        CoreApiResponse RegisterUser(string username, string password, string languageui);
        CoreApiResponse AuthenticateUser(string username, string password);
        CoreApiResponse AuthenticateUserByConfirmationCode(string code);
        CoreApiResponse RecoverUserPassword(string userName);
        CoreApiResponse ChangeUserPassword(string username, string password);
        CoreApiResponse GetKey(ApiKeyRequestObject requestObject);
        CoreApiResponse GetUser(int Id);
        CoreApiResponse GetRole(string username);
        CoreApiResponse AddRole(string username, string role);
        CoreApiResponse UserSecretKey(string username);
        #endregion
    }
}