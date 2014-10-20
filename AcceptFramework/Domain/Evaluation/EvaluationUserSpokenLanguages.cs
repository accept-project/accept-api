namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationUserSpokenLanguages : DomainBase
    {
        public virtual int UserID { get; set; }
        public virtual int LanguageID { get; set; }
    }
}
