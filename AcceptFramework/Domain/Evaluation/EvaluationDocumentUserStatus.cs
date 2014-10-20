using System;
using AcceptFramework.Domain.Common;
using AcceptPortal.Models.Evaluation;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationDocumentUserStatus : DomainBase
    {
        public virtual EvaluationDocument Document { get; set; }
        public virtual User User { get; set; }
        public virtual EvaluationStatus Status { get; set; }
        public virtual DateTime CompletionDate { get; set; }
        public virtual double CompletionTime { get; set; }
        public virtual EvaluationDirection Direction { get; set; }
    }
}