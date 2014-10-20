using System.Collections.Generic;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationSourceSegment : DomainBase
    {
        public virtual EvaluationSourceParagraph SourceParagraph { get; set; }
        public virtual IList<EvaluationScoring> SourceScorings { get; set; }
        public virtual IList<EvaluationScoring> BilingualScorings { get; set; } 
        public virtual IList<EvaluationSourceParagraph> SourceParagraphs { get; set; }

        public virtual int SourceParagraphID { get; set; }

        public virtual string SourceString { get; set; }
        public virtual int WordCount { get; set; }
        public virtual int Length { get; set; }
        public virtual int IQScore { get; set; }
        public virtual int SpellCheckFlags { get; set; }
        
        public virtual string TargetString { get; set; }    
        public virtual int ComprehensibilityScore { get; set; } 
        public virtual int FidelityScore { get; set; } 

        public EvaluationSourceSegment()
        {
            SourceParagraphs = new List<EvaluationSourceParagraph>();
            SourceScorings = new List<EvaluationScoring>();
            BilingualScorings = new List<EvaluationScoring>();
        }

    }
}
