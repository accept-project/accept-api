using System;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationTargetSegmentScoring : DomainBase
    {
        public virtual EvaluationTargetSegment TargetSegment { get; set; }
        public virtual User User { get; set; }

        public virtual int TargetSegmentID { get { return TargetSegment.Id; } }
        public virtual int UserID{get { return User.Id; }}
        public virtual int ComprehensibilityScore { get; set; }
        public virtual string ComprehensibilityComment { get; set; }
        public virtual int FidelityScore { get; set; }
        public virtual string FidelityComment { get; set; }
        public virtual DateTime ScoringDate { get; set; }

        public EvaluationTargetSegmentScoring()
        {
           
        }
    }
}
