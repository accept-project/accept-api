using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.Authentication
{
    public class ApiKeyRequestObject
    {
        public ApiKeyRequestObject()
        {
            User = string.Empty;
            Password = string.Empty;
            Key = string.Empty;
            Status = -1;
            Dns = string.Empty;
            Ip = string.Empty;

            AplicationName = string.Empty;
            Organization = string.Empty;
            Description = string.Empty;
        }

        public string User { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
        public int Status { get; set; }
        public string Dns { get; set; }
        public string Ip { get; set; }

        public string AplicationName { get; set; }
        public string Organization { get; set; }
        public string Description { get; set; }

      
    }
}