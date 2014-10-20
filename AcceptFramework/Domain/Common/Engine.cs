using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Common
{
    public class Engine: DomainBase
    {
        public virtual string EngineName { get; set; }
        public virtual int Status { get; set; }
        public virtual bool Enable { get; set; }
        public virtual string Address { get; set; }
    }
}
