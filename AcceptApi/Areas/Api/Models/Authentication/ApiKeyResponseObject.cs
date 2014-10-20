using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptApi.Areas.Api.Models.Core;
using System.Runtime.Serialization;
using AcceptFramework.Domain.Common;

namespace AcceptApi.Areas.Api.Models.Authentication
{
    [Serializable]
    [DataContract]
    public class ApiKeyResponseObject: CoreApiResponse 
    {
        [DataMember(Name = "KeyList", IsRequired = false)]
        public List<ApiKeys> KeyList { get; set; }      

        public ApiKeyResponseObject()
        :base(CoreApiResponseStatus.Ok,DateTime.UtcNow,string.Empty) {
            KeyList = new List<ApiKeys>();
        }

        public ApiKeyResponseObject(List<ApiKeys> keys)
            : base(CoreApiResponseStatus.Ok, DateTime.UtcNow, string.Empty)
        {
            KeyList = keys;
        }


        

    }
}