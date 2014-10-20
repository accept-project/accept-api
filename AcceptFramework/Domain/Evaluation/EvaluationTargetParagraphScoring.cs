using System;
using AcceptFramework.Domain.Common;
using AcceptPortal.Models.Evaluation;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationTargetParagraphScoring : DomainBase
    {
        public virtual EvaluationSourceParagraph SourceParagraph { get; set; }
        public virtual User User { get; set; }
        public virtual EvaluationStatus Status { get; set; }

        public virtual int SourceParagraphID { get; set; }
        public virtual int UserID { get; set; }
        public virtual int ComprehensibilityScore { get; set; }
        public virtual string ComprehensibilityComment { get; set; }
        public virtual int FidelityScore { get; set; }
        public virtual string FidelityComment { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime CompleteDate { get; set; }

        public EvaluationTargetParagraphScoring()
        {
           
        }
    }
}
