using AcceptApi.Areas.Api.Models.Core;
using AcceptPortal.Models.Evaluation;

namespace AcceptApi.Areas.Api.Models.Interfaces
{
    public interface IEvaluationManager
    {
        CoreApiResponse Documents(int id);
        CoreApiResponse UploadFile(int projectId, int provider, int langpair, string file);
        CoreApiResponse SimpleScore(int Id, string key, string answerId, string domain, string var1, string var2, string var3, string var4, string var5, string var6, string var7, string var8, string var9, string var10);
        //method added for IE issue.
        string SimpleScoreFormPost(int Id, string key, string answerId, string domain, string var1, string var2, string var3, string var4, string var5, string var6, string var7, string var8, string var9, string var10);
        CoreApiResponse CreateQuestion(int projectId, string name);
        CoreApiResponse CreateQuestionItem(int projectId, int qid, int lid, string name, string action, string confirmation);
        CoreApiResponse CreateQuestionItemAnswer(int projectId, int qid, string answer, string value);        
        CoreApiResponse CreateProject(string name, string description, string org, string domain, string requestor);
        CoreApiResponse UpdateProject(int Id, string name, string description, string org, string apiKey, string domain);
        CoreApiResponse DeleteProject(int Id);
        CoreApiResponse GetProject(int projectId);
        CoreApiResponse GetAllProjects();
        CoreApiResponse GetProjectsByUser(string username);
        CoreApiResponse GetQuestion(int id);
        CoreApiResponse GetQuestion(int id, string key, string language, string category, string question);
        CoreApiResponse GetAllLanguages();
        CoreApiResponse GetAllQuestions(int Id);
        CoreApiResponse GetAllQuestions(int id, string key, string language, string category, string question, string domain);
        CoreApiResponse GetAllScores(int Id, string token);
        CoreApiResponse DeleteAnswer(int Id);
        CoreApiResponse UpdateQuestionAnswer(int Id, string answer, string value);
        CoreApiResponse DeleteCategory(int Id);
        CoreApiResponse UpdateQuestionItem(int Id, int lid, string name, string action, string confirmation);
        CoreApiResponse DeleteQuestion(int Id);
        CoreApiResponse UpdateQuestionCategory(int Id, string name);
        CoreApiResponse AddContentToProjectRaw(string jsonRaw, int projectId);
        CoreApiResponse ContentChunks(int Id, string key, string language, string category, string question, string domain);
    }
}
