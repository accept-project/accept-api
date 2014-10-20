using System;
using System.Text.RegularExpressions;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationSimpleScore: DomainBase
    {
        public virtual int ProjectID { get; set; }
        public virtual string Domain { get; set; }
        public virtual int QuestionCategoryId { get; set; }
        public virtual string QuestionCategory { get; set; }
        public virtual int QuestionId { get; set; }
        public virtual string Question { get; set; }
        public virtual int AnswerId { get; set; }
        public virtual string AnswerValue { get; set; }
        public virtual string Answer { get; set; }
        public virtual string Language { get; set; }
        public virtual DateTime TimeStamp { get; set; }
        public virtual string Var1 { get; set; }
        public virtual string Var2 { get; set; }
        public virtual string Var3 { get; set; }
        public virtual string Var4 { get; set; }
        public virtual string Var5 { get; set; }
        public virtual string Var6 { get; set; }
        public virtual string Var7 { get; set; }
        public virtual string Var8 { get; set; }
        public virtual string Var9 { get; set; }
        public virtual string Var10 { get; set; }

        public EvaluationSimpleScore() { }

        public EvaluationSimpleScore(int _projectId, int _questionCategoryId, string _questionCategory, int _questionId,
            string _question, int _answerId, string _answer, string _domain, string _language, string _var1, string _var2, 
            string _var3, string _var4, string _var5, string _var6, string _var7, string _var8, string _var9, string _var10)
        {
      
            ProjectID = _projectId;
            QuestionCategoryId = _questionCategoryId;
            QuestionCategory = _questionCategory;
            QuestionId = _questionId;
            Question = _question;
            AnswerId = _answerId;
            Answer = _answer;
            Domain = _domain;
            Language = _language;
            Var1 = _var1;
            Var2 = _var2;
            Var3 = _var3;
            Var4 = _var4;
            Var5 = _var5;
            Var6 = _var6;
            Var7 = _var7;
            Var8 = _var8;
            Var9 = _var9;
            Var10 = _var10;
            TimeStamp = DateTime.Now;
        }

    }

    public class EvaluationSimpleScoreResponse
    {
        public int ProjectID { get; set; }
        public int ScoreID { get; set; }
        public bool Success { get; set; }

        public EvaluationSimpleScoreResponse()
        {
        }

        public EvaluationSimpleScoreResponse(int _projectId, int _scoreId, bool _success)
        {
            ProjectID = _projectId;
            ScoreID = _scoreId;
            Success = _success;
        }

    }

}