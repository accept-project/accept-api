using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEdit
{
    public class TargetSentence:DomainBase
    {

        public TargetSentence()
        {
            text = string.Empty;
            options = new List<TargetSentenceOption>();
            lastComment = string.Empty;
            lastOption = string.Empty;
        }

        public TargetSentence(string text, List<TargetSentenceOption> options)
        {
            this.text = text;
            this.options = options;
            lastComment = string.Empty;
            lastOption = string.Empty;
        }

        public virtual string text { get; set; }
        public virtual ICollection<TargetSentenceOption> options { get; set; }
        public virtual string lastComment { get; set; }
        public virtual string lastOption { get; set; }
    }
}
