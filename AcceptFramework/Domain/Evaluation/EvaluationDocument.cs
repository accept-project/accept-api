using System;
using System.Collections.Generic;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationDocument : DomainBase, ICloneable
    {
        public virtual string Name { get; set; }

        public virtual EvaluationProject Project { get; set; }
        public virtual EvaluationProvider Provider { get; set; }
        public virtual EvaluationLanguagePair LanguagePair { get; set; }
        public virtual IList<EvaluationSourceParagraph> Paragraphs { get; set; }
        public virtual IList<EvaluationDocumentUserStatus> UserStatuses { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual bool IsFull { get; set; }
        public virtual double CompletionTime { get; set; }
        public virtual DateTime CompletedDate { get; set; }

        public EvaluationDocument()
        {
            Paragraphs = new List<EvaluationSourceParagraph>();
            UserStatuses = new List<EvaluationDocumentUserStatus>();
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}