using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.PostEdit
{
    public class DocumentRetrieveObject
    {
        public string textId { get; set; }
        public string userId { get; set; }

        public DocumentRetrieveObject()
        {
            this.textId = string.Empty;
            this.userId = string.Empty;        
        }
    
    }
}