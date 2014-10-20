namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationParagraphStatistic
    {
        public int SegmentID { get; set; }
        public string SourceString { get; set; }
        public string TargetString { get; set; }
        public int ComprehensibilityScore { get; set; }
        public int FidelityScore { get; set; }
        public int ScoringID { get; set; }
    }
}
