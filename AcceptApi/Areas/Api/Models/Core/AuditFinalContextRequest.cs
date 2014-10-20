using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.Core
{
    public class AuditFinalContextRequest
    {
       public string globalSessionId {get;set;}
       public string textContent { get; set; }
       public string timeStamp { get; set; }

       public AuditFinalContextRequest() {

           this.globalSessionId = string.Empty;
           this.textContent = string.Empty;
           this.timeStamp = string.Empty;
       
       }
    
    }
}