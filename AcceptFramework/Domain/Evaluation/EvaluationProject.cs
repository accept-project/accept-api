using System;
using System.Collections.Generic;
using AcceptFramework.Domain.Common;
using AcceptPortal.Models.Evaluation;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationProject : DomainBase
    {
        public virtual string Name { get; set; }
        public virtual string Organization { get; set; }
        public virtual string Description { get; set; }
        public virtual string ApiKey { get; set; }
        public virtual string Domain { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual User Creator { get; set; }
        public virtual EvaluationStatus Status { get; set; }        
        public virtual EvaluationType Type { get; set; }
        public virtual IList<EvaluationProvider> Providers { get; set; }
        public virtual IList<EvaluationLanguagePair> LanguagePairs { get; set; }
        public virtual IList<EvaluationQuestion> Questions { get; set; }        
        public virtual EvaluationProjectMode Mode { get; set; }
        public virtual ICollection<EvaluationContentChunk> ContentChunks { get; set; }
        public virtual string AdminToken { get; set; }

        public EvaluationProject()
        {
            Providers = new List<EvaluationProvider>();
            Questions = new List<EvaluationQuestion>();
            LanguagePairs = new List<EvaluationLanguagePair>();
            Status = EvaluationStatus.InProgress;
            Mode = EvaluationProjectMode.Source | EvaluationProjectMode.Bilingual | EvaluationProjectMode.Target;
            CreationDate = DateTime.MinValue;
            this.ContentChunks = new List<EvaluationContentChunk>();
            this.AdminToken = string.Empty;
        }

        public EvaluationProject(string name, EvaluationType type)
        {
            Name = name; 
            Type = type;
            this.AdminToken = string.Empty;
            this.ContentChunks = new List<EvaluationContentChunk>();
        }

    }
}