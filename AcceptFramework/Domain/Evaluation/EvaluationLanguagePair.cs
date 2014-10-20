
namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationLanguagePair : DomainBase
    {
        public virtual EvaluationLanguage SourceLanguage { get; set; }
        public virtual EvaluationLanguage TargetLanguage { get; set; }
        public virtual bool Active { get; set; }        

        public virtual int SourceLanguageID { get { return SourceLanguage.Id; } }
        public virtual int TargetLanguageID { get { return TargetLanguage.Id; } }

        public EvaluationLanguagePair()
        {
            SourceLanguage = new EvaluationLanguage();
            TargetLanguage = new EvaluationLanguage();
        }

        public virtual string FullName
        {
            get
            {
                return string.Format("{0} - {1}", SourceLanguage.Name, TargetLanguage.Name);
            }
        }

        public override string ToString()
        {
            return FullName;
        }  
    }
}