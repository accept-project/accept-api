using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Common
{
    public class Domain: DomainBase
    {
        public virtual Universe DomainUniverse { get; set; }
        public virtual IList<Language> Languages { get; set; }
        public virtual string DomainName { get; set; }       
        public virtual int Status { get; set; }

        public Domain(string name, Universe universe, IList<Language> languages)
        {
            DomainName = name;
            Languages = languages;
            DomainUniverse = universe;
            Status = 1;
            Languages = languages;
        }

        public Domain()
        {
            DomainName = string.Empty;
            DomainUniverse = null;
            Languages = new List<Language>();
            Status = 1;
        }

    }
}
