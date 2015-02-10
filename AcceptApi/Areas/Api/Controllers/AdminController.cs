using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcceptApi.Areas.Api.Models.Interfaces;
using AcceptApi.Areas.Api.Models.Managers;
using AcceptApi.Areas.Api.Models.PostEdit;
using AcceptApi.Areas.Api.Models.Filters;
using AcceptApi.Areas.Api.Models.Core;

namespace AcceptApi.Areas.Api.Controllers
{
    [CrossSiteJsonCall]
    public class AdminController : Controller
    {       
        private IAdminManager adminManager;
        private IPostEditManager postEditManager;

        public IPostEditManager PostEditManager
        {
            get { return postEditManager; }
        }

        public AdminController()
        {
            this.adminManager = new AdminManager();
            this.postEditManager = new PostEditManager();
        }

        protected IAdminManager AdminManager
        {
            get { return adminManager; }            
        }


        #region Universe

        [HttpPost]
        public JsonResult CreateUniverse(string name)
        {
            var model = AdminManager.CreateUniverse(name);
            return Json(model);
        }
        [HttpGet]
        public JsonResult GetUniverse(int  Id)
        {
            var model = AdminManager.GetUniverse(Id);
            return Json(model, JsonRequestBehavior.AllowGet);          
        }
       

         [HttpGet]
        public JsonResult GetAllUniverse()
        {
            var model = AdminManager.GetAllUniverse();
            return Json(model, JsonRequestBehavior.AllowGet);          
        }

        #endregion

        #region Domains

        [HttpPost]
        public JsonResult CreateDomain(string name, int universeId)
        {
            var model = AdminManager.CreateDomain(name, universeId);
            return Json(model);
        }

        [HttpGet]
        public JsonResult GetDomain(int Id)
        {
            var model = AdminManager.GetDomain(Id);
            return Json(model, JsonRequestBehavior.AllowGet);   
        }


        [HttpGet]
        public JsonResult GetDomainsByUniverse(int Id)
        {
            var model = AdminManager.GetDomainsByUniverse(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAllDomains()
        {
            var model = AdminManager.GetAllDomains();
            return Json(model, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Projects

        [HttpPost]
        public JsonResult CreateProject(string name,int domainId, int status)
        {
            var model = AdminManager.CreateProject(name, domainId, status);
            return Json(model);
        }

        [RestHttpVerbFilter]
        public JsonResult Project(ProjectRequestObject project, string httpVerb)
        {
            switch (httpVerb)
            {
                case "POST": 
                {
                   var model = AdminManager.CreateProject(project);
                   return Json(model);
                
                }
                case "PUT":
                {
                   var model = AdminManager.UpdateProject(project);
                   return Json(model);
                }
                default: 
                {                    
                    return Json("Invalid Http Verb");                
                }
            }
          
        }

        [HttpGet]
        public JsonResult IsProjectStarted(int? projectId)
        {
            var model = AdminManager.IsProjectStarted(projectId.GetValueOrDefault(-1));
            return Json(model, JsonRequestBehavior.AllowGet);        
        }

        [HttpPost]
        public JsonResult ProjectByUser(string userName)
        {
            var model = AdminManager.GetProjectsByUser(userName);
            return Json(model);
        }

        [HttpGet]
        public JsonResult GetProject(int Id)
        {
            var model = AdminManager.GetProject(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetProjectsByDomain(int Id)
        {
            var model = AdminManager.GetProjectsByDomain(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllProjects()
        {
            var model = AdminManager.GetAllProjects();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DemoProjects()
        {
            var model = AdminManager.GetDemoProjects();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteProject(int Id)
        {
            var model = AdminManager.UpdateProjectStatus(Id, -1);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUserProject(string userName, string token)
        {
            var model = AdminManager.AddUserToProject(userName, "ProjUser", token);
            return Json(model);
        }

        [RestHttpVerbFilter]
        public JsonResult UserProject(string userName, string token, string httpVerb)
        {
            try
            {
                switch (httpVerb)
                {
                    case "GET":
                    {   
                        var model = AdminManager.GetUsersInProject(token, "*");
                        return Json(model, JsonRequestBehavior.AllowGet);                       
                    }
                    case "POST":
                    {
                        var model = AdminManager.AddUserToProject(userName, "ProjUser", token);
                        return Json(model);
                    }
                    case "DELETE":
                    {
                        var model = AdminManager.RemoveUserFromProject(userName, "ProjUser", token);
                        return Json(model);
                    }
                    case "OPTIONS": { return Json("{}"); }
                    default: { throw new Exception("Http verb not allowed."); } 
                }
            }
            catch (Exception e)
            {
                return Json(new CoreApiException(e.Message, "UserProject"));
            }                      
        }

        [HttpGet]
        public JsonResult ProjectInfo(string token)
        {
            var model = AdminManager.GetProjectGeneralInfo(token);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #region External Project Methods
        
        [HttpGet]
        public JsonResult ProjectTaskStatus(string token)
        {
            var model = PostEditManager.GetProjectTaskStatus(-1, token);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
       
        #endregion

        #endregion

        #region Users

        [HttpPost]
        public JsonResult AddUserDomain(string userName, int domainId, string userRole)
        {
            var model = AdminManager.AddUserToDomain(userName, domainId, userRole);
            return Json(model);
        }

        [RestHttpVerbFilter]
        public JsonResult UserRolePostEditProject(string userName, int projectId, string role, string httpVerb)
        {
            switch(httpVerb)
            {
                case "GET":
                {
                    var model = AdminManager.UserRolePostEditProject(userName,projectId,role);
                    return Json(model, JsonRequestBehavior.AllowGet);
                
                }                                    
            }
                        
            return Json(new CoreApiException("Http verb not implemented.","UserRolePostEditProject"), JsonRequestBehavior.AllowGet);        
        }

        [HttpGet]
        public JsonResult GetAllUsers()
        {
            try 
            {	        
		        var model = AdminManager.GetAllUsers();         
                return Json(model, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception e)
            {
                return Json(new CoreApiException(e.Message, "GetAllUsers"), JsonRequestBehavior.AllowGet);
            }

        }       

        #endregion

        #region User Feedback

        [HttpPost]
        public JsonResult UserFeedback(string user, string email, string message, string link, string subject)
        {
            var model = AdminManager.SendFeedbackEmail(user,email,link,message, subject);
            return Json(model);                
        }

        #endregion


        [HttpGet]
        public JsonResult CreateDbSchema()
        {
            var model = AdminManager.GetAllUsers();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult InitialiseAccept()
        {
            var model = AdminManager.InitAccept();
            return Json(model, JsonRequestBehavior.AllowGet);
        }       

    }
}
