using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.Core
{
    public class CoreApiResponseStatus
    {
        public const string NotSet = "NOSET";
        public const string Ok = "OK";
        public const string Failed = "FAILED";
        public const string Error = "ERROR";
        public const string Incomplete = "INCOMPLETE";  
    }
}