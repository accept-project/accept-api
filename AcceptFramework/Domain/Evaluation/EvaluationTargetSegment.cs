using System.Collections.Generic;
using System.Linq;


namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationTargetSegment : DomainBase
    {
        public virtual EvaluationSourceParagraph SourceParagraph { get; set; }
        public virtual EvaluationReferenceSegment ReferenceSegment { get; set; }
        public virtual IList<EvaluationScoring> TargetScorings { get; set; }

        public virtual int SourceParagraphID { get; set; }
        public virtual int ReferenceSegmentID { get; set; }

        public virtual EvaluationSourceSegment SourceSegment { get; set; }
        public virtual string TargetString { get; set; }
        public virtual int WordCount { get; set; }
        public virtual int Length { get; set; }
        public virtual double GTM { get; set; }
        public virtual double BLEU { get; set; }
        public virtual double TER { get; set; }
        public virtual double Meteor { get; set; }
        public virtual int EditDistance { get; set; }
        public virtual int IQScore { get; set; }
        public virtual int SpellCheckFlags{ get; set; }
        
        public EvaluationTargetSegment()
        {
            TargetScorings = new List<EvaluationScoring>();
        }

    }
}
