using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Core
{
    [DataContract]
    [Serializable]
    public class CoreApiGlobalSessionReportObject
    {
        [DataMember(Name="meta")]
        public Meta MetaData { get; set; }
        [DataMember(Name = "input")]
        public string Input { get; set; }
        [DataMember(Name = "output")]
        public string Output { get; set; }
        [DataMember(Name = "child_sessions")]
        public List<ChildSession> ChildSessions { get; set; }
        [DataMember(Name = "global_start_time")]
        public string GlobalStartTime { get; set; }
        [DataMember(Name = "global_end_time")]
        public string GlobalEndTime { get; set; }

        public CoreApiGlobalSessionReportObject()
        {
            this.MetaData = new Meta();
            this.Input = string.Empty;
            this.Output = string.Empty;
            this.ChildSessions = new List<ChildSession>();
            this.GlobalStartTime = string.Empty;
            this.GlobalEndTime = string.Empty;
        }
    }

    [DataContract]
    [Serializable]    
    public class Meta 
    {
        [DataMember(Name="rule_set")]
        public string RuleSet { get; set; }
        [DataMember(Name = "language")]
        public string Language { get; set; }
        [DataMember(Name = "global_session_id")]
        public string GlobalSessionId { get; set; }
        [DataMember(Name = "user")]
        public string User { get; set; }
        [DataMember(Name = "OriginUrl")]
        public string OriginUrl { get; set; }
       
        public Meta()
        {
            this.RuleSet = string.Empty;
            this.Language = string.Empty;
            this.GlobalSessionId = string.Empty;
            this.User = string.Empty;
            this.OriginUrl = string.Empty;        
        }

    }

    [DataContract]
    [Serializable]        
    public class ChildSession
    {
        [DataMember(Name = "context")]
        public string Context { get; set; }       
        [DataMember(Name = "results")]
        public List<ReportFlag> Results {get;set;}
        [DataMember(Name = "providerResults")]
        public List<object> ProviderResults { get; set; }
        [DataMember(Name = "clientResults")]
        public List<ClientReportFlag> ClientResults { get; set; }

        public ChildSession()
        {            
            this.Context = string.Empty;
            this.Results = new List<ReportFlag>();
            this.ProviderResults = new List<object>();
            this.ClientResults = new List<ClientReportFlag>();
        }

    }

    public class ClientReportFlag : BaseReportFlag
    {         
    }

    [DataContract]
    [Serializable]
    public class BaseReportFlag
    {
        [DataMember(Name = "flag")]
        public string FlagContext { get; set; }
        [DataMember(Name = "action")]
        public string Action { get; set; }
        [DataMember(Name = "action_value")]
        public string ActionValue { get; set; }       
        [DataMember(Name = "name")]
        public string Name { get; set; }
        public BaseReportFlag() {
            this.FlagContext = string.Empty;
            this.Action = string.Empty;
            this.ActionValue = string.Empty;           
            this.Name = string.Empty;                    
        }
    }
    
    public class ReportFlag:BaseReportFlag
    {        
        public int IndexStart { get; set; }
        public int IndexEnd { get; set; }

        public ReportFlag()
        {
            this.IndexStart = -1;
            this.IndexEnd = -1;
        }
    }
}