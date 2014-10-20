namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationProvider : DomainBase
    {
        public virtual string Name { get; set; }

        /// <summary>
        /// Parent project relation
        /// </summary>
        public virtual bool? Active { get; set; }
    }
}