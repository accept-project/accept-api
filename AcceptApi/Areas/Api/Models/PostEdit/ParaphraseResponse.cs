using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptApi.Areas.Api.Models.Core;

namespace AcceptApi.Areas.Api.Models.PostEdit
{
    public class ParaphraseResponse
    {
        public virtual string sourceText { get; set; }
        public virtual string status { get; set; }
        public virtual string error { get; set; }       
        public virtual List<string[]> resultSet { get; set;}

        public ParaphraseResponse()
        {
            this.sourceText = string.Empty;
            this.resultSet = new List<string[]>();

            this.status = string.Empty;
            this.error = string.Empty;
        
        }
    }
}