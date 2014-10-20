using System;
using AcceptApi.Areas.Api.Models.Interfaces;
using AcceptApi.Areas.Api.Models.Core;
using AcceptFramework.Interfaces.Evaluation;
using AcceptFramework.Interfaces;
using AcceptFramework.Business;
using AcceptPortal.Models.Evaluation;
using AcceptApi.Areas.Api.Models.Evaluation;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AcceptFramework.Domain.Evaluation;
using System.Collections.Generic;

namespace AcceptApi.Areas.Api.Models.Managers
{
    public class EvaluationManager: IEvaluationManager
    {
        private IAcceptApiServiceLocator _acceptServiceLocator;
        private IEvaluationProjectManager _evaluationProjectManagerService;

        public EvaluationManager()
        {

            _acceptServiceLocator = new AcceptApiServiceLocator();
            _evaluationProjectManagerService = _acceptServiceLocator.GetEvaluationProjectManagerService();
        }

        #region Properties


        public IEvaluationProjectManager EvaluationProjectManagerService
        {
            get { return _evaluationProjectManagerService; }
        }

        #endregion

        #region Projects

        public CoreApiResponse CreateProject(string name, string description, string org, string domain, string requestor)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.CreateProject(name, description, org, domain, requestor));
               
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateEvalProject");
            }

        }

        public CoreApiResponse UpdateProject(int Id, string name, string description, string org, string apiKey, string domain)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.UpdateProject(Id, name, description, org, apiKey, domain));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "UpdateProject");
            }
        }

        public CoreApiResponse DeleteProject(int Id)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.DeleteProject(Id));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "DeleteProject");
            }
        }


        public CoreApiResponse CreateQuestion(int projectId, string name)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.CreateQuestion(projectId, name));

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateEvalQuestion");
            }
        }

        public CoreApiResponse CreateQuestionItem(int projectId, int qid, int lid, string name, string action, string confirmation)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.CreateQuestionItem(projectId, qid, lid, name, action, confirmation));

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateEvalQuestionItem");
            }
        }

        public CoreApiResponse CreateQuestionItemAnswer(int projectId, int qid, string answer, string value)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.CreateQuestionItemAnswer(projectId, qid, answer, value));

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateEvalQuestionItemAnswer");
            }
        }

        public CoreApiResponse UploadFile(int projectId, int provider, int langpair, string file)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.UploadFile(projectId, provider, langpair, file));

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "UploadFile");
            }

        }

        public CoreApiResponse Documents(int id)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.Documents(id));

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "Documents");
            }

        }

        public CoreApiResponse SimpleScore(int Id, string key, string answerId, string domain, string var1, string var2, string var3, string var4, string var5, string var6, string var7, string var8, string var9, string var10)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.SimpleScore(Id, key, answerId, domain, var1, var2, var3, var4, var5, var6, var7, var8, var9, var10));

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateEvalSimpleScore");
            }

        }

        //Method added for IE issue
        public string SimpleScoreFormPost(int Id, string key, string answerId, string domain, string var1, string var2, string var3, string var4, string var5, string var6, string var7, string var8, string var9, string var10)
        {
            EvaluationProjectManagerService.SimpleScore(Id, key, answerId, domain, var1, var2, var3, var4, var5, var6, var7, var8, var9, var10);
            return "OK";
        }

        public CoreApiResponse GetProject(int projectId)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.GetProject(projectId));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetProject");
            }

        }

        public CoreApiResponse GetAllProjects()
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.GetAllProjects());
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllProjects");
            }

        }

        public CoreApiResponse GetProjectsByUser(string username)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.GetProjectsByUser(username));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetProjectsByUser");
            }

        }

        public CoreApiResponse GetQuestion(int id)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.GetQuestion(id));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetQuestion");
            }

        }

        public CoreApiResponse GetQuestion(int id, string key, string language, string category, string question)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.GetQuestion(id));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetQuestion");
            }

        }


        public CoreApiResponse GetAllQuestions(int Id)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.GetAllQuestions(Id));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllQuestions");
            }

        }

        public CoreApiResponse GetAllLanguages()
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.GetAllLanguages());
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllLanguages");
            }

        }

        public CoreApiResponse GetAllQuestions(int Id, string key, string language, string category, string question, string domain)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.GetAllQuestions(Id, key, language, category, question, domain));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllQuestions");
            }

        }


        public CoreApiResponse ContentChunks(int Id, string key, string language, string category, string question, string domain)
        {
            try
            {               

               EvaluationProject evaluationProject = EvaluationProjectManagerService.GetProject(Id);
               List<EvaluationQuestion> evaluationQuestions = EvaluationProjectManagerService.GetAllQuestions(Id, key, language, category, question, domain);
               var apiResponse = new { @chunkList = evaluationProject.ContentChunks, @questionList = evaluationQuestions };

               return new CoreApiCustomResponse(apiResponse);
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllQuestions");
            }

        }

        public CoreApiResponse GetAllScores(int Id, string token)
        {
            try
            {
                EvaluationProject evaluationProject = EvaluationProjectManagerService.GetProject(Id);
                if (token != evaluationProject.AdminToken)
                    throw new ArgumentException("Invalid Project Token.", "token");


                return new CoreApiCustomResponse(EvaluationProjectManagerService.GetAllScores(Id));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllScores");
            }

        }

        public CoreApiResponse DeleteAnswer(int Id)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.DeleteAnswer(Id));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "DeleteAnswer");
            }
        }

        public CoreApiResponse UpdateQuestionAnswer(int Id, string answer, string value)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.UpdateQuestionAnswer(Id, answer, value));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "UpdateQuestionAnswer");
            }
        }

        public CoreApiResponse DeleteCategory(int Id)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.DeleteCategory(Id));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "DeleteCategory");
            }
        }

        public CoreApiResponse UpdateQuestionItem(int Id, int lid, string name, string action, string confirmation)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.UpdateQuestionItem(Id, lid, name, action, confirmation));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "UpdateQuestionItem");
            }
        }

        public CoreApiResponse DeleteQuestion(int Id)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.DeleteQuestion(Id));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "DeleteQuestion");
            }
        }

        public CoreApiResponse UpdateQuestionCategory(int Id, string name)
        {
            try
            {
                return new CoreApiCustomResponse(EvaluationProjectManagerService.UpdateQuestionCategory(Id, name));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "UpdateQuestion");
            }
        }
        
        public CoreApiResponse AddContentToProjectRaw(string jsonRaw, int projectId)
        {
            try
            {
                EvaluationProject evaluationProject =  EvaluationProjectManagerService.GetProject(projectId);
                if (evaluationProject == null)
                    return new CoreApiException("Evaluation project not found.", "AddContentToProjectRaw");

                EvaluationContentChunkDocument doc = new EvaluationContentChunkDocument();

                try
                {                   
                    doc = JsonConvert.DeserializeObject<EvaluationContentChunkDocument>(jsonRaw);
                }
                catch (Exception ex1)
                {
                    return new CoreApiException(ex1.Message, "AddContentToProjectRaw-InvalidJson");
                }

                try
                {
                    if (evaluationProject.ContentChunks.Count > 0)
                    {
                        evaluationProject.ContentChunks = new List<EvaluationContentChunk>();
                        EvaluationProjectManagerService.UpdateProject(evaluationProject);
                    }

                    foreach (ContentChunk cchunks in doc.chunkList)
                    {
                        EvaluationContentChunk newContentChunk = new EvaluationContentChunk();
                        newContentChunk.Chunk = cchunks.chunk;
                        newContentChunk.ChunkInfo = cchunks.chunkInfo;
                        newContentChunk.Status = cchunks.active;                        
                        newContentChunk.Type = 1;

                        try
                        {                            
                            evaluationProject.ContentChunks.Add(EvaluationProjectManagerService.InsertContentChunk(newContentChunk));
                        }
                        catch
                        { 
                        
                        }

                    }

                    EvaluationProjectManagerService.UpdateProject(evaluationProject);
                }
                catch (Exception ex2)
                {
                    return new CoreApiException(ex2.Message, "AddContentToProjectRaw-ParsingChunks");
                }               
                return new CoreApiResponse();                               
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "AddContentToProjectRaw");
            }
        
        
        }


        public CoreApiResponse AddContentToProject(EvaluationContentChunkDocument document)
        {
            try
            {
                return new CoreApiResponse();
            }
            catch (Exception e)
            {                                
                return new CoreApiException(e.Message, "AddContentToProject");
            }
        }

        #endregion

    }
}