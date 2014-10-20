
namespace AcceptFramework.Domain.Evaluation
{
    public class ProjectActiveLanguagePair : DomainBase
    {
        public virtual int ProjectID { get; set; }
        public virtual int LanguagePairID { get; set; }
    }

    public class EvaluationProjectLanguagePair : DomainBase
    {
        public virtual int ProjectID { get; set; }
        public virtual int LanguagePairID { get; set; }
    }
}