using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.Authentication
{
    public class UserRequestObject
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string LanguageUI { get; set; }
    }
}