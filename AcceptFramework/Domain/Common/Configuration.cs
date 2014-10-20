using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Common
{
    public class Configuration: DomainBase
    {
        public virtual Engine Engine { get; set; }
        public virtual string Context { get; set; }
        public virtual string Address { get; set; }
        public virtual string CheckType { get; set; }
    }
}
