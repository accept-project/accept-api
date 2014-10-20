using System.Collections.Generic;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationSourceParagraph : DomainBase
    {
        public virtual EvaluationDocument Document { get; set; }
        public virtual EvaluationAuthorType AuthorType { get; set; }
        public virtual EvaluationCategory Category { get; set; }

        public virtual int ExternalPostID { get; set; }
        public virtual string Original { get; set; }

        public virtual int DocumentID { get; set; }
        public virtual int AuthorTypeID { get; set; }
        public virtual int CategoryID { get; set; }

        public virtual IList<EvaluationSourceSegment> SourceSegments { get; set; }
        public virtual IList<EvaluationScoring> Scorings { get; set; }
        public virtual IList<EvaluationTargetSegment> TargetSegments { get; set; }

        public virtual int SegmentsNumber { get; set; }
        public virtual double CompletionTime { get; set; }
        public virtual bool Completed { get; set; }

        public virtual string DocumentDirection { get; set; }
        public virtual string SourceHash { get; set; }

        public EvaluationSourceParagraph()
        {
            SourceSegments = new List<EvaluationSourceSegment>();
            Scorings = new List<EvaluationScoring>();
            TargetSegments = new List<EvaluationTargetSegment>();
        }
    }
}
