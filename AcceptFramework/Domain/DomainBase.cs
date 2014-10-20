using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain
{
    public abstract class DomainBase
    {
        public virtual int Id { get;  set; }
        public virtual void Validate() { }
    }
}
