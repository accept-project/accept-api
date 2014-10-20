using System;
using AcceptPortal.Models.Evaluation;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationDocumentStatistic
    {
        public string ProjectName { get; set; }
        public string ProviderName { get; set; }
        public int DocumentID { get; set; }
        public EvaluationLanguagePair LanguagePair { get; set; }
        public Direction Direction { get; set; }
        public double CompletionTime { get; set; }
        public DateTime CompletedDate { get; set; }
        public double AvgParaCompScore { get; set; }
        public double AvgSegmentCompScore { get; set; }
        public double ParaFidelity { get; set; }
        public double SegmentFidelity { get; set; }
        public double DocumentComprehensibility { get; set; }
        public double DocumentFidelity { get; set; }
    }
}