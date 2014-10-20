using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Common;
using AcceptFramework.Domain.Evaluation;
using AcceptFramework.Repository;
using AcceptFramework.Repository.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationSourceParagraphRepository : RepositoryBase<EvaluationSourceParagraph>
    {
        public List<EvaluationSourceParagraph> SelectByDocumentID(int documentID, string direction, User CurrentUser)
        {
                EvaluationDocument document = Select(d => d.Id == documentID).SingleOrDefault().Document;
                if (document == null)
                {
                    return new List<EvaluationSourceParagraph>();
                }

                return document.Paragraphs.Select(p => new EvaluationSourceParagraph
                {
                    Id = p.Id,
                    SegmentsNumber = p.SourceSegments.Count,
                    Completed = CheckComplete(p, direction, CurrentUser),
                    CompletionTime = CheckCompletionTime(p, direction),
                    DocumentDirection = direction
                   
                }).ToList();
        }

        private bool CheckComplete(EvaluationSourceParagraph paragraph, string direction, User CurrentUser)
        {
            EvaluationParagraphScoring ps = null;
            if(direction == "Both") //both
                ps = new EvaluationParagraphScoringRepository().Select(
                        p => p.SourceParagraph == paragraph && p.Completed && p.Scoring.User.Id == CurrentUser.Id && p.Scoring.LanguagePair != null).
                        FirstOrDefault();
            else if (direction == "Source") //source
                ps = new EvaluationParagraphScoringRepository().Select(
                        p => p.SourceParagraph == paragraph && p.Completed && p.Scoring.User.Id == CurrentUser.Id && p.Scoring.SourceLanguage != null).
                        FirstOrDefault();
            else if (direction == "Target") //target
                ps = new EvaluationParagraphScoringRepository().Select(
                        p => p.SourceParagraph == paragraph && p.Completed && p.Scoring.User.Id == CurrentUser.Id && p.Scoring.TargetLanguage != null).
                        FirstOrDefault();
            return ps != null;
        }

        private double CheckCompletionTime(EvaluationSourceParagraph paragraph, string direction)
        {
            EvaluationParagraphScoring ps = null;
            if (direction == "Both") //both
                ps = new EvaluationParagraphScoringRepository().Select(
                        p => p.SourceParagraph == paragraph && p.Completed && p.Scoring.LanguagePair != null).
                        FirstOrDefault();
            else if (direction == "Source") //source
                ps = new EvaluationParagraphScoringRepository().Select(
                        p => p.SourceParagraph == paragraph && p.Completed && p.Scoring.SourceLanguage != null).
                        FirstOrDefault();
            else if (direction == "Target") //target
                ps = new EvaluationParagraphScoringRepository().Select(
                        p => p.SourceParagraph == paragraph && p.Completed && p.Scoring.TargetLanguage != null).
                        FirstOrDefault();
            if (ps != null)
                return ps.CompletionTime;
            return 0;
        }

        
    }
}
