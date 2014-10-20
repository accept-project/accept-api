using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Mapping.PostEdit;

namespace AcceptFramework.Domain.PostEdit
{
    public class TargetSentenceOption:DomainBase
    {

        public virtual string context { get; set; }
        public virtual ICollection<OptionValueObject> values { get; set; }

        public TargetSentenceOption()
        {
            context = string.Empty;
            values = new List<OptionValueObject>();
        }

        public TargetSentenceOption(string context, List<OptionValueObject> optvalues)
        {
            this.context = context;
            this.values = optvalues;
        }

    }
}
