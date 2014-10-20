using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Core
{
    [Serializable]
    [DataContract]
    public class AcceptApiException: CoreApiResponse
    {

        public string Exception { get; set; }
        public string Context { get; set; }

        public AcceptApiException(string exception, string context)
            : base(CoreApiResponseStatus.Failed, DateTime.Now)
        {
            Exception = exception;
            Context = context;
        }

        public AcceptApiException()
            : base(CoreApiResponseStatus.NotSet, DateTime.Now)
        {

        }                
    }
}