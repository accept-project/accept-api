using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Core
{
    [DataContract]
    public class CoreApiRequest
    {

        public CoreApiRequest()
        {
            this.User = string.Empty;
            this.Password = string.Empty;
            this.Text = string.Empty;
            this.Language = string.Empty;
            this.Rule = string.Empty;
            this.Grammar = string.Empty;
            this.Spell = string.Empty;
            this.Style = string.Empty;
            this.RequestFormat = string.Empty;
            this.ApiKey = string.Empty;
            this.GlobalSessionId = string.Empty;
            this.IEDomain = string.Empty;
            this.SessionMetadata = string.Empty;
        }

        [DataMember]
        public string User { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Language { get; set; }
        [DataMember]
        public string Rule { get; set; }
        [DataMember]
        public string Grammar { get; set; }
        [DataMember]
        public string Spell { get; set; }
        [DataMember]
        public string Style { get; set; }
        [DataMember]
        public string RequestFormat { get; set; }
        [DataMember]
        public string ApiKey { get; set; }
        [DataMember]
        public string GlobalSessionId { get; set; }
        [DataMember]
        public string IEDomain { get; set; }
        [DataMember]
        public string SessionMetadata { get; set; }

    }
}