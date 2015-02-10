using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class TranslationUnit: DomainBase
    {
        public virtual int SegmentIndex { get; set; }                       
        public virtual string State {get; set;}       
        public virtual string PhaseName { get; set; }
        public virtual string Source { get; set; }
        public virtual string Target { get; set; }
        public virtual ICollection<AlternativeTranslation> AlternativeTranslations {get; set;}

        public TranslationUnit()
        {
            AlternativeTranslations = new List<AlternativeTranslation>();
        
        }

        public TranslationUnit(int index, string state, string phaseName, string source, string target, List<AlternativeTranslation> alternativeTranslations)
        {
            this.SegmentIndex = index;    
            this.State = state;
            this.PhaseName = phaseName;
            this.Source = source;
            this.Target = target;
            AlternativeTranslations = alternativeTranslations;
        }



    }
}
