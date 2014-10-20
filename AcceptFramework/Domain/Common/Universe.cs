using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Common
{
    public class Universe: DomainBase
    {
        public virtual string UniverseName { get; set; }
        public virtual int Status { get; set; }
        public Universe() { UniverseName = string.Empty; Status = 1; }
        public Universe(string name) { UniverseName = name; Status = 1; }
    }
}
