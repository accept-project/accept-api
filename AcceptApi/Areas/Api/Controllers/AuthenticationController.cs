using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcceptApi.Areas.Api.Models.Filters;
using AcceptApi.Areas.Api.Models.Authentication;
using AcceptApi.Areas.Api.Models;
using AcceptApi.Areas.Api.Models.Core;

namespace AcceptApi.Areas.Api.Controllers
{
    [CrossSiteJsonCall]
    public class AuthenticationController : Controller
    {       
        private IAcceptApiManager _acceptCoreManager;

        public IAcceptApiManager AcceptCoreManager
        {
            get { return _acceptCoreManager; }
        }

        public AuthenticationController()
        {

            this._acceptCoreManager = new AcceptApiManager();
        }

        #region  api keys

        [RestHttpVerbFilter]
        public JsonResult Key(ApiKeyRequestObject requestObject, string httpVerb)
        {
            var model = new CoreApiResponse(CoreApiResponseStatus.Failed,DateTime.UtcNow,"");
            switch (httpVerb)
            {
                case "POST": { model = AcceptCoreManager.CreateUserKey(requestObject); } break;
                case "GET": 
                {
                    model = AcceptCoreManager.GetKey(requestObject);
                    return Json(model, JsonRequestBehavior.AllowGet);
                } 
                case "PUT": { model = AcceptCoreManager.UpdateUserKey(requestObject); } break;
                case "DELETE": 
                { 
                    model = AcceptCoreManager.DeleteUserKey(requestObject); 
                
                }break;
                default: 
                {                     
                    return Json(model);                                    
                }                         
            }

            return Json(model);
        }

       [RestHttpVerbFilter]
        public JsonResult GetKey(ApiKeyRequestObject requestObject, string httpVerb)
        {
            var model = AcceptCoreManager.GetKey(requestObject);
            return Json(model, JsonRequestBehavior.AllowGet);
                  
        }        

        [RestHttpVerbFilter]
        public JsonResult UserKeys(ApiKeyRequestObject requestObject, string httpVerb)
        {
            var model = AcceptCoreManager.GetUserKeys(requestObject);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UserSecretKey(string user)
        {
            var model = AcceptCoreManager.UserSecretKey(user);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region user

        [HttpGet]
        public JsonResult GetUser(int Id)
        {
            var model = AcceptCoreManager.GetUser(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AuthenticateUser(string username, string password)
        {
            var model = AcceptCoreManager.AuthenticateUser(username, password);
            return Json(model);
        }

        [RestHttpVerbFilter]
        public JsonResult Role(string userName, string roleName, string httpVerb)
        {
            var model = new CoreApiException("Invalid Http Verb", "Role");

            switch (httpVerb)
            {
                case "GET": { return Json(AcceptCoreManager.GetRole(userName),  JsonRequestBehavior.AllowGet); }
                case "POST": { return Json(AcceptCoreManager.AddRole(userName,roleName)); } 
            
            }
           
            return Json(model);
        }

        [HttpPost]
        public JsonResult RegisterUser(string username, string password, string languageui)
        {
            var model = AcceptCoreManager.RegisterUser(username, password, languageui);
            return Json(model);
        }

        [HttpPost]
        public JsonResult AuthenticateUserByConfirmationCode(string code)
        {
            var model = AcceptCoreManager.AuthenticateUserByConfirmationCode(code);
            return Json(model);
        }

        [HttpPost]
        public JsonResult RecoverUserPassword(string username)
        {
            var model = AcceptCoreManager.RecoverUserPassword(username);
            return Json(model);
        }

        [HttpPost]
        public JsonResult ChangeUserPassword(string username, string password)
        {
            var model = AcceptCoreManager.ChangeUserPassword(username, password);
            return Json(model);
        }

        #endregion              
    }
}
