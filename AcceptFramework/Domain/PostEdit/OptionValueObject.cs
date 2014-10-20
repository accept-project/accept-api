using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEdit
{
    public class OptionValueObject : DomainBase
    {
        public virtual string phrase { get; set; }
        public virtual System.Nullable<int> start { get; set; }
        public virtual System.Nullable<int> end { get; set; }
        public virtual System.Nullable<float> fscore { get; set; }

        public OptionValueObject() {

            phrase = string.Empty;
            start = -1;
            end = -1;
            fscore = 0;        
        }    

        public OptionValueObject(string phrase, float fscore, int start, int end)
        {
            this.phrase = phrase;
            this.fscore = fscore;
            this.start = start;
            this.end = end;
        }

    }
}
