using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Acrolinx
{

    [DataContract]
    [Serializable]
    public class AcrolinxServerCapabilitiesObject
    {
        public AcrolinxServerCapabilitiesObject()
        {
            this.languageId = string.Empty;
            this.customAdmittedTermFlagName = string.Empty;
            this.reuseHarvestingSentenceBankIds = new List<string>();
            this.ruleSetCapabilities = new List<RuleSetCapabilities>();        
        }

        [DataMember]
        public string languageId { get; set; }
        [DataMember]
        public string customAdmittedTermFlagName { get; set; }
        [DataMember]
        public List<string> reuseHarvestingSentenceBankIds { get; set; }
        [DataMember]
        public List<RuleSetCapabilities> ruleSetCapabilities { get; set; }
    }

    [DataContract]
    [Serializable]
    public class RuleSetCapabilities
    {
        public RuleSetCapabilities()
        {
            this.name = string.Empty;
            this.contextInfoNames = new List<string>();
            this.useHardExclusion = string.Empty;
            this.flagTypes = new List<string>();
            this.maxRulePriority = 0;        
        }

        [DataMember]
        public string name { get; set; }
        [DataMember]
        public List<string> contextInfoNames { get; set; }
        [DataMember]
        public string useHardExclusion { get; set; }
        [DataMember]
        public List<string> flagTypes { get; set; }
        [DataMember]
        public int maxRulePriority { get; set; }
        [DataMember]
        public List<string> termSetNames { get; set; }
            
    }


}
