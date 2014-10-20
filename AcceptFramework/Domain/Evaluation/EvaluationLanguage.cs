using System.Text.RegularExpressions;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationLanguage: DomainBase
    {

        public virtual string Name { get; set; }
        public virtual string Code { get; set; }

        public EvaluationLanguage()
        {
        }

        public EvaluationLanguage(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public virtual void Language(string langFullName)
        {
            const string pattern = @"(?<name>\w*)\s*?\((?<code>\w*)\)";
            Match langMatch = Regex.Match(langFullName, pattern);
            
            if (langMatch.Success)
            {
                Name = langMatch.Groups["name"].Value.Trim();
                Code = langMatch.Groups["code"].Value.Trim();
            }
        }
   
    
    }
}