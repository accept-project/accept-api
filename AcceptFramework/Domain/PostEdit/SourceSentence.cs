using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEdit
{
    public class SourceSentence : DomainBase
    {
        public SourceSentence() { text = string.Empty; }
        public SourceSentence(string text) { this.text = text; }
        public virtual string text { get; set; }       
    }
}
