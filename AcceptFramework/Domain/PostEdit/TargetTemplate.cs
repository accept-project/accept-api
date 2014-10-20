using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEdit
{
    public class TargetTemplate: DomainBase
    {
        public virtual string markup { get; set; }      
        public TargetTemplate() { markup = string.Empty; }
        public TargetTemplate(string markup) { this.markup = markup; }
    }
}
