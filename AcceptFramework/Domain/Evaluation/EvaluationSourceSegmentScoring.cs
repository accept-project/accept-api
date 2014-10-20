using System;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationSourceSegmentScoring : DomainBase
    {
        public virtual EvaluationSourceSegment SourceSegment { get; set; }
        public virtual User User { get; set; }

        public virtual int SourceSegmentID { get { return SourceSegment.Id; } }
        public virtual int UserID{get { return User.Id; }}
        public virtual int ComprehensibilityScore { get; set; }
        public virtual string ComprehensibilityComment { get; set; }
        public virtual int FidelityScore { get; set; }
        public virtual string FidelityComment { get; set; }
        public virtual DateTime ScoringDate { get; set; }
        public virtual int Timestamp { get; set; } 

        public EvaluationSourceSegmentScoring()
        {
           
        }
    }
}
