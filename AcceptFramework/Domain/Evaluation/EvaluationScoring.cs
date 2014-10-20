using System;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationScoring : DomainBase 
    {
        public virtual EvaluationLanguagePair LanguagePair { get; set; }
        public virtual EvaluationLanguage SourceLanguage { get; set; }
        public virtual EvaluationLanguage TargetLanguage { get; set; }
        public virtual User User { get; set; }
        public virtual EvaluationProvider Provider { get; set; }

        public virtual int LanguagePairID { get; set; }
        public virtual int SourceLanguageID { get; set; }
        public virtual int TargetLanguageID { get; set; }
        public virtual int UserID { get; set; }
        public virtual int ProviderID { get; set; }

        public virtual int ComprehensibilityScore { get; set; }
        public virtual string ComprehensibilityComment { get; set; }
        public virtual int FidelityScore { get; set; }
        public virtual string FidelityComment { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime ScoringDate { get; set; }
        public virtual int Timestamp { get; set; }
    }
}
