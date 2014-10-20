
namespace AcceptFramework.Domain.Evaluation
{
    public class ProjectActiveProvider : DomainBase
    {
        public virtual int ProjectID { get; set; }
        public virtual int ProviderID { get; set; }
    }

    public class EvaluationProjectProvider : DomainBase
    {
        public virtual int ProjectID { get; set; }
        public virtual int ProviderID { get; set; }
    }
}