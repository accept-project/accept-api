using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class TranslationRevision: DomainBase
    {
        public virtual int SegmentIndex { get; set; }                
        public virtual string State {get; set;}      
        public virtual string PhaseName { get; set; }
        public virtual string Source { get; set; }
        public virtual string Target { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime LastUpdate { get; set; }
        public virtual ICollection<Phase> Phases { get; private set; }
        public virtual ICollection<AlternativeTranslation> AlternativeTranslations { get; private set; }
        public virtual ICollection<ThinkPhase> ThinkPhases { get; private set; }

        public TranslationRevision()
        {
            this.Phases = new List<Phase>();
            this.AlternativeTranslations = new List<AlternativeTranslation>();
            this.ThinkPhases = new List<ThinkPhase>();
        }

        public TranslationRevision(int index, string state, string phaseName, string source, string target, DateTime dateCreated, List<Phase> translationRevisionPhases, DateTime lastUpdate)
        {
            this.SegmentIndex = index;          
            this.State = state;
            this.PhaseName = phaseName;
            this.Source = source;
            this.Target = target;
            this.DateCreated = dateCreated;
            this.LastUpdate = lastUpdate;
            this.Phases = translationRevisionPhases;
            this.ThinkPhases = new List<ThinkPhase>();
            this.AlternativeTranslations = new List<AlternativeTranslation>();
        }

        public TranslationRevision(int index, string state, string phaseName, string source, string target, DateTime dateCreated, List<Phase> translationRevisionPhases, DateTime lastUpdate, List<AlternativeTranslation> alternativeTranslations)
        {
            this.SegmentIndex = index;
            this.State = state;
            this.PhaseName = phaseName;
            this.Source = source;
            this.Target = target;
            this.DateCreated = dateCreated;
            this.LastUpdate = lastUpdate;
            this.Phases = translationRevisionPhases;
            this.AlternativeTranslations = alternativeTranslations;
            this.ThinkPhases = new List<ThinkPhase>();
            this.AlternativeTranslations = new List<AlternativeTranslation>();

        }


    }
}
