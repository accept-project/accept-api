using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Core
{

    [DataContract]
    [Serializable]
    public class AcceptApiResponseStatus: CoreApiResponse
    {
      
            public AcceptApiResponseStatus()
            {
                State = string.Empty;                       
            }

            public AcceptApiResponseStatus(string state, string sessionCodeId)
            :base(CoreApiResponseStatus.Ok,DateTime.Now,sessionCodeId)
            {
                State = state;                                 
            }

            [DataMember(Name = "State", IsRequired = false)]
            public string State { get; set; }                     
    
    }
}