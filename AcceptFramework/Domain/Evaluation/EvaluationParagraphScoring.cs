namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationParagraphScoring : DomainBase
    {
        public virtual EvaluationSourceParagraph SourceParagraph { get; set; }
        public virtual EvaluationScoring Scoring { get; set; }
        public virtual int SourceParagraphID { get; set; }
        public virtual int ScoringID { get; set; }
        public virtual bool Completed { get; set; }
        public virtual double CompletionTime { get; set; }
    }
}
