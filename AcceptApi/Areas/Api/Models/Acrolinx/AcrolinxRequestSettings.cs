using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using AcceptApi.Areas.Api.Models.Core;

namespace AcceptApi.Areas.Api.Models.Acrolinx
{

    [DataContract]
    [Serializable]
    public class AcrolinxRequestSettings
    {

        [DataMember(Name = "request", IsRequired = false)]
        public string Request { get; set; }

        [DataMember(Name = "sessionId", IsRequired = false)]
        public string SessionId { get; set; }

        [DataMember(Name = "checkPriority", IsRequired = false)]
        public string CheckPriority { get; set; }

        [DataMember(Name = "checkReportFormats", IsRequired = false)]
        public string[] CheckReportFormats { get; set; }

        [DataMember(Name = "requestFormat", IsRequired = false)]
        public string RequestFormat { get; set; }

        [DataMember(Name = "requestedCheckResultTypes", IsRequired = false)]
        public string[] RequestedCheckResultTypes { get; set; }

        [DataMember(Name = "clientLocale", IsRequired = false)]
        public string ClientLocale { get; set; }     
        
        [DataMember(Name = "requestDescription", IsRequired = false)]
        public RequestDescription RequestDescription { get; set; }

        [DataMember(Name = "checkSettings", IsRequired = false)]
        public CheckSettings CheckSettings { get; set; }   
        
        [DataMember(Name = "metaInfo", IsRequired = false)]
        public MetaInfo MetaInfo { get; set; }  
        
        
    }


    [DataContract]
    [Serializable]
    public class RequestDescription
    {
        public RequestDescription(string id, string name, string author, string format, bool complete, string scope)
        {
            this.Id = id;
            this.Name = name;
            this.Author = author;
            this.Format = format;
            this.IsComplete = complete;
            this.Scope = scope;    
        }


        [DataMember(Name="id", IsRequired=false)]
        public string Id { get; set; }

        [DataMember(Name = "name", IsRequired = false)]
        public string Name { get; set; }

        [DataMember(Name = "author", IsRequired = false)]
        public string Author { get; set; }

        [DataMember(Name = "format", IsRequired = false)]
        public string Format { get; set; }

        [DataMember(Name = "isComplete", IsRequired = false)]
        public bool IsComplete { get; set; }

        [DataMember(Name = "scope", IsRequired = false)]
        public string Scope { get; set; }
    
    }

    [DataContract]
    [Serializable]
    public class CheckSettings
    {
        public CheckSettings()
        { }

        public CheckSettings(string languageid, string ruleSetName, string[] requestedflagtypes, string[] termsetnames)
        {
            this.LanguageId = languageid;
            this.RuleSetName = ruleSetName;
            this.RequestedFlagTypes = requestedflagtypes;
            this.TermSetNames = termsetnames;                
        }

        [DataMember(Name = "languageId", IsRequired = false)]
        public string LanguageId { get; set; }

        [DataMember(Name = "ruleSetName", IsRequired = false)]
        public string RuleSetName { get; set; }

        [DataMember(Name = "requestedFlagTypes", IsRequired = false)]
        public string[] RequestedFlagTypes { get; set; }

        [DataMember(Name = "termSetNames", IsRequired = false)]
        public string[] TermSetNames { get; set; }

    }

    [DataContract]
    [Serializable]
    public class MetaInfo
    {
        public MetaInfo(string[] keys, string[] values)
        {
            this.Keys = keys;
            this.Values = values;
        }

        [DataMember(Name = "keys", IsRequired = false)]
        public string[] Keys { get; set; }

        [DataMember(Name = "values", IsRequired = false)]
        public string[] Values { get; set; }
    }
    
}