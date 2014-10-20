using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Core
{
    [Serializable]
    [DataContract]
    public class CoreApiException: CoreApiResponse
    {
        public string Exception { get; set; }
        public string Context { get; set; }
        public CoreApiException(string exception, string context)
            : base(CoreApiResponseStatus.Failed, DateTime.Now)
        {
            Exception = exception;
            Context = context;
        }

        public CoreApiException()
            : base(CoreApiResponseStatus.NotSet, DateTime.Now)
        {

        }                
    }
}