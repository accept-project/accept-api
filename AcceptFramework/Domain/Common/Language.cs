using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Common
{
    public class Language: DomainBase
    {
        public virtual Configuration Configuration { get; set; }
        public virtual string LanguageName { get; set; }
        public virtual string LanguageCode { get; set; }
    }
}
