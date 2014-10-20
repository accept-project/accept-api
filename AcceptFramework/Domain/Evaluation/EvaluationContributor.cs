using System.Collections.Generic;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationContributor : DomainBase
    {
        public virtual EvaluationLanguage Language { get; set; }
        public virtual string ContributorName { get; set; }
        public virtual int LanguageID { get; set; }
        public virtual List<EvaluationReferenceSegment> ReferenceSegments { get; set; }
        
        public EvaluationContributor()
        {
            ReferenceSegments = new List<EvaluationReferenceSegment>();
        }
    }
}
