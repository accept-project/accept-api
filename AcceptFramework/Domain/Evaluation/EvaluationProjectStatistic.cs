using System;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationProjectStatistic
    {
        public EvaluationProject Project { get; set; }
        public EvaluationLanguagePair LanguagePair { get; set; }
        public string LanguagePairString { get; set; }
        public EvaluationProvider Provider { get; set; }
        public int ScoringsCount { get; set; }
        public int DocumentsCount { get; set; }
        public double AverageComprehensibilityScoreParagraph { get; set; }
        public double AverageFidelityScoreParagraph { get; set; }
        public double AverageComprehensibilityScoreSegment { get; set; }
        public double AverageFidelityScoreSegment { get; set; }
        public double DocumentComprehensibility { get; set; }
        public double DocumentFidelity { get; set; }
        public Direction Direction { get; set; }

        public EvaluationProjectStatistic Clone()
        {
            return (EvaluationProjectStatistic) MemberwiseClone();
        }
    }
}