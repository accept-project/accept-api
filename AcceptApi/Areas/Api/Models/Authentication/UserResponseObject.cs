using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptApi.Areas.Api.Models.Core;

namespace AcceptApi.Areas.Api.Models.Authentication
{
    public class UserResponseObject : CoreApiResponse
    {
        public string UserName { get; set; }       
        public string UILanguage { get; set; }        
        public string Role { get; set; }

        public UserResponseObject(string userName, string uILanguage)
        {
            this.UserName = userName;
            this.UILanguage = uILanguage;
        }

        public UserResponseObject(string userName, string uILanguage, string role)
        {
            this.UserName = userName;
            this.UILanguage = uILanguage;
            this.Role = role;
        }

    }
}