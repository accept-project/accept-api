using System.Collections.Generic;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationReferenceSegment : DomainBase
    {
        public virtual EvaluationContributor Contributor { get; set; }
        public virtual int ContributorID { get; set; }
        public virtual string ReferenceString { get; set; }
        public virtual IList<EvaluationTargetSegment> TargetSegments { get; set; }

        public EvaluationReferenceSegment()
        {
            TargetSegments = new List<EvaluationTargetSegment>();
        }
    }
}
