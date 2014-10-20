using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.Core
{
    public class AuditFlagRequest
    {
      public string globalSessionId {get;set;}
      public string flag { get; set; }
      public string userAction { get; set; }
      public string actionValue { get; set; }
      public string ignored { get; set; }
      public string name { get; set; }
      public string textBefore { get; set; }
      public string textAfter { get; set; }
      public string timeStamp { get; set; }
      public string jsonRaw { get; set; }
      public string privateId { get; set; }

      public AuditFlagRequest()
      {
          this.globalSessionId = string.Empty;
          this.flag = string.Empty;
          this.userAction = string.Empty;
          this.actionValue = string.Empty;
          this.ignored = string.Empty;
          this.name = string.Empty;
          this.textBefore = string.Empty;
          this.textAfter = string.Empty;
          this.timeStamp = string.Empty;
          this.jsonRaw = string.Empty;
          this.privateId = string.Empty;
      }        
    
    
    }
}