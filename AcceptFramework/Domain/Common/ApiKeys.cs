using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Common
{
    public class ApiKeys : DomainBase
    {

        public ApiKeys()
        {
            ApiKey = string.Empty;
            CreationDate = DateTime.MinValue;
            KeyDns = string.Empty;
            KeyIp = string.Empty;
            KeyStatus = 1;

            ApplicationName = string.Empty;
            Organization = string.Empty;
            Description = string.Empty;
           
        }

        public virtual string ApiKey { get; set; }

        public virtual DateTime CreationDate { get; set; }
        
        public virtual string KeyDns { get; set; }

        public virtual string KeyIp { get; set; }

        public virtual int KeyStatus { get; set; }

        public virtual string ApplicationName { get; set; }
        public virtual string Organization { get; set; }        
        public virtual string Description { get; set; }
       
    
    }
}
