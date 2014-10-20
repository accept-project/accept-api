using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AcceptApi.Areas.Api.Models.ActionResults;
using AcceptApi.Areas.Api.Models.Core;
using AcceptApi.Areas.Api.Models.Interfaces;
using AcceptApi.Areas.Api.Models.Managers;
using AcceptApi.Areas.Api.Models.Utils;
using AcceptPortal.Models.Evaluation;
using AcceptApi.Areas.Api.Models.Filters;
using System.Web;
using AcceptApi.Areas.Api.Models.Evaluation;

namespace AcceptApi.Areas.Api.Controllers
{
    [CrossSiteJsonCall]
    public class EvaluationController : Controller
    {
        private IEvaluationManager _evaluationManager;

        public EvaluationController()
        {
            _evaluationManager = new EvaluationManager();
        }

        protected IEvaluationManager EvaluationManager
        {
            get { return _evaluationManager; }            
        }

        [HttpPost]
        public JsonResult UploadFile(int projectId, int provider, int langpair, string file)

        {
            var model = EvaluationManager.UploadFile(projectId, provider, langpair, file);
            return Json(model);
        }

        [HttpPost]
        public JsonResult CreateQuestion(int projectId, string name)
        {
            var model = EvaluationManager.CreateQuestion(projectId, name);
            return Json(model);
        }

        [HttpPost]
        public JsonResult CreateQuestionItem(int projectId, int qid, int lid, string name, string action, string confirmation)
        {
            var model = EvaluationManager.CreateQuestionItem(projectId, qid, lid, name, action, confirmation);
            return Json(model);
        }

        [HttpPost]
        public JsonResult CreateQuestionItemAnswer(int projectId, int qid, string answer, string value)
        {
            var model = EvaluationManager.CreateQuestionItemAnswer(projectId, qid, answer, value);
            return Json(model);
        }

        [HttpGet]
        public JsonResult Documents(int Id)
        {
            var model = EvaluationManager.Documents(Id);
            return Json(model);
        }

        [HttpPost]
        public JsonResult CreateProject(string name, string description, string org, string domain, string requestor)
        {
            var model = EvaluationManager.CreateProject(name, description, org, domain, requestor);
            return Json(model);
        }

        [HttpPost]
        public JsonResult UpdateProject(int Id, string name, string description, string org, string apikey, string domain)
        {
            var model = EvaluationManager.UpdateProject(Id, name, description, org, apikey, domain);
            return Json(model);
        }

        [HttpGet]
        public JsonResult GetProject(int Id)
        {
            var model = EvaluationManager.GetProject(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllProjects()
        {
            var model = EvaluationManager.GetAllProjects();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProjectsByUser(string userName)
        {
            var model = EvaluationManager.GetProjectsByUser(userName);
            return Json(model);
        }

        [HttpGet]
        public JsonResult GetQuestion(int Id)
        {
            var model = EvaluationManager.GetQuestion(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
                        
        [HttpPost]
        public JsonResult UpdateQuestionAnswer(int Id, string name, string value)
        {
            var model = EvaluationManager.UpdateQuestionAnswer(Id, name, value);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateQuestion(int Id, int lid, string name, string action, string confirmation)
        {
            var model = EvaluationManager.UpdateQuestionItem(Id, lid, name, action, confirmation);
            return Json(model);
        }

        [HttpPost]
        public JsonResult UpdateQuestionCategory(int Id, string name)
        {
            var model = EvaluationManager.UpdateQuestionCategory(Id, name);
            return Json(model);
        }

        [HttpDelete]
        public JsonResult DeleteProject(int Id)
        {
            var model = EvaluationManager.DeleteProject(Id);
            return Json(model);
        }

        [HttpDelete]
        public JsonResult DeleteCategory(int Id)
        {
            var model = EvaluationManager.DeleteCategory(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpDelete]
        public JsonResult DeleteQuestion(int Id)
        {
            var model = EvaluationManager.DeleteQuestion(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteAnswer(int Id)
        {
            var model = EvaluationManager.DeleteAnswer(Id);
            return Json(model);
        }

        [HttpGet]
        public JsonResult GetAllQuestions(int Id)
        {
            var model = EvaluationManager.GetAllQuestions(Id);
            JsonResult res = Json(model, JsonRequestBehavior.AllowGet);
            return res;    
        }

        [HttpGet]
        public JsonResult Scores(int Id, string token)
        {
            string domain = GetDomain();
            var model = EvaluationManager.GetAllScores(Id, token);
            JsonResult res = Json(model, JsonRequestBehavior.AllowGet);
            return res;
        }

        [HttpGet]
        public JsonpResult Questions(int Id, string key, string language, string category, string question)
        {
            string domain = GetDomain();
            var model = EvaluationManager.GetAllQuestions(Id, key, language, category, question, domain); 
            return new JsonpResult(model);
        }

        [HttpGet]
        public JsonpResult ContentChunks(int Id, string key, string language, string category, string question)
        {
            string domain = GetDomain();
            var model = EvaluationManager.ContentChunks(Id, key, language, category, question, domain);
            return new JsonpResult(model);
        }

        [HttpGet]
        public JsonResult Languages()
        {            
            var model = EvaluationManager.GetAllLanguages();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [RestHttpVerbFilter]
        public JsonResult Score(int Id, string key, string answer, string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10, string httpVerb)
        {
            if (httpVerb == "POST")
            {
                string domain = GetDomain();

                param1 = ReplaceParameter(param1);
                param2 = ReplaceParameter(param2);
                param3 = ReplaceParameter(param3);
                param4 = ReplaceParameter(param4);
                param5 = ReplaceParameter(param5);
                param6 = ReplaceParameter(param6);
                param7 = ReplaceParameter(param7);
                param8 = ReplaceParameter(param8);
                param9 = ReplaceParameter(param9);
                param10 = ReplaceParameter(param10);

                var model = EvaluationManager.SimpleScore(Id, key, answer, domain, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
                JsonResult res = Json(model);
                return res;
            }

            return Json(new CoreApiException("Invalid Http Verb", "Score"));
        }
        
        //ie fix.
        [HttpPost]
        public string ScoreFormPost(int? Id, string key, string answer, string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10)
        {
            try
            {                   
                string domain = GetDomain();
                param1 = ReplaceParameter(param1);
                param2 = ReplaceParameter(param2);
                param3 = ReplaceParameter(param3);
                param4 = ReplaceParameter(param4);
                param5 = ReplaceParameter(param5);
                param6 = ReplaceParameter(param6);
                param7 = ReplaceParameter(param7);
                param8 = ReplaceParameter(param8);
                param9 = ReplaceParameter(param9);
                param10 = ReplaceParameter(param10);
                string model = EvaluationManager.SimpleScoreFormPost(Id.GetValueOrDefault(), key, answer, domain, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);              
                return model;
            }
            catch (Exception e)
            {
                return "FAILED:" + e.Message;
            }
                         
        }

        [HttpGet]
        public JsonpResult ScoreP(int Id, string key, string answer, string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10)
        {           
                string domain = GetDomain();              
                param1 = ReplaceParameter(param1);
                param2 = ReplaceParameter(param2);
                param3 = ReplaceParameter(param3);
                param4 = ReplaceParameter(param4);
                param5 = ReplaceParameter(param5);
                param6 = ReplaceParameter(param6);
                param7 = ReplaceParameter(param7);
                param8 = ReplaceParameter(param8);
                param9 = ReplaceParameter(param9);
                param10 = ReplaceParameter(param10);
                var model = EvaluationManager.SimpleScore(Id, key, answer, domain, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
                JsonResult res = Json(model);
                return new JsonpResult (res);                      
        }

        [HttpPost]
        public JsonResult AddContentToProjectRaw(string jsonRaw, int projectId)
        {            
            string decoded = HttpUtility.UrlDecode(jsonRaw, System.Text.Encoding.UTF8);
            var model = EvaluationManager.AddContentToProjectRaw(decoded, projectId);
            return Json(model);         
        }
       
        [HttpPost]        
        public JsonResult AddContentToProject(string token, EvaluationContentChunkDocument document)
        {            
            return Json("");
        }

        private string ReplaceParameter(string param)
        {          
            if (param == "{IP}")
            {
                param = Request.UserHostAddress;
            }
            return param;
        }

        private string GetDomain()
        {
            string domain = "";

            domain = AcceptApiWebUtils.GetUrlReferrerHost();

            if (domain == "")
            {
                domain = Request.UserHostAddress;
            }

            return domain;
        }
    }
}
