using System.Collections.Generic;
using AcceptFramework.Domain.Evaluation;
using AcceptPortal.Models.Evaluation;

namespace AcceptFramework.Interfaces.Evaluation
{
    public interface IEvaluationProjectManager
    {
        List<EvaluationDocumentSimple> Documents(int id);

        bool GenerateLanguages();

        bool UploadFile(int projectId, int provider, int langpair, string file);
        
        EvaluationSimpleScoreResponse SimpleScore(int Id, string key, string answerId, string domain, string var1, string var2, string var3, string var4, string var5, string var6, string var7, string var8, string var9, string var10);

        EvaluationQuestion CreateQuestion(int projectId, string name);
        
        EvaluationQuestionItem CreateQuestionItem(int projectId, int qid, int lid, string name, string action, string confirmation);
        
        EvaluationQuestionItemAnswer CreateQuestionItemAnswer(int projectId, int qid, string answer, string value);

        EvaluationProject CreateProject(string name, string description, string org, string domain, string requestor);
        
        EvaluationProject UpdateProject(int projectId, string name, string description, string org, string apiKey, string domain);
        
        bool DeleteProject(int id);

        EvaluationProject GetProject(int projectId);

        List<EvaluationProject> GetAllProjects();
        
        List<EvaluationProject> GetProjectsByUser(string username);

        EvaluationQuestion GetQuestion(int id, string key, string language, string category, string question);
        
        EvaluationQuestion GetQuestion(int id);

        List<EvaluationLanguage> GetAllLanguages();

        List<EvaluationQuestion> GetAllQuestions(int Id, string key, string language, string category, string question, string domain);
        
        List<EvaluationQuestion> GetAllQuestions(int Id);
        
        List<EvaluationSimpleScore> GetAllScores(int Id);

        bool DeleteAnswer(int id);
        
        EvaluationQuestionItemAnswer UpdateQuestionAnswer(int Id, string answer, string value);

        bool DeleteCategory(int id);
        
        EvaluationQuestionItem UpdateQuestionItem(int id, int lid, string name, string action, string confirmation);

        bool DeleteQuestion(int id);
        
        EvaluationQuestion UpdateQuestionCategory(int Id, string name);

        EvaluationContentChunk InsertContentChunk(EvaluationContentChunk newContentChunk);
        
        EvaluationProject UpdateProject(EvaluationProject project);

    }
}
