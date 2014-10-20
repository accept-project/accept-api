using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Core
{
    [Serializable]
    [DataContract]
    public class CoreApiResponse
    {
        [DataMember(Name = "responseStatus", IsRequired = false, Order = 1)]
        public string ResponseStatus { get; set; }
        [DataMember(Name = "timeStamp", IsRequired = false, Order = 1)]
        public DateTime TimeStamp { get; set; }        
        /// <summary>
        /// this attribute should be moved to more detailed object such as PreEditCoreApiObject or so - since there core api responses that don't need session ID.
        /// </summary>
        [DataMember(Name = "session", IsRequired = false)]
        public string AcceptSessionCode { get; set; }
        /// <summary>
        /// this attribute should be moved to more detailed object such as PreEditCoreApiObject or so - since there core api responses that don't need globalSessionId ID.
        /// </summary>        
        [DataMember(Name = "globalSessionId", IsRequired = false)]
        public string GlobalSessionId { get; set; }

        public CoreApiResponse(string coreApiResponseStatus ,DateTime timeStamp, string sessionCodeid)
        {
            ResponseStatus = coreApiResponseStatus;
            TimeStamp = timeStamp;
            AcceptSessionCode = sessionCodeid;
            GlobalSessionId = string.Empty;
        }

        public CoreApiResponse()
        {
            ResponseStatus = CoreApiResponseStatus.Ok;
            TimeStamp = DateTime.Now;
            AcceptSessionCode = string.Empty;
            GlobalSessionId = string.Empty;
        }

        public CoreApiResponse(string responseStatus, DateTime timeStamp)
        {
            ResponseStatus = responseStatus;
            TimeStamp = timeStamp;
            AcceptSessionCode = string.Empty;
            GlobalSessionId = string.Empty;
        }
    
    }
}