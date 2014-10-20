using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.Core
{
    public  class CoreApiCustomResponse: CoreApiResponse
    {
        private object _responseObject;


        public CoreApiCustomResponse() { }


        public CoreApiCustomResponse(object responseObject) {
            _responseObject = responseObject;
        }


        public object ResponseObject
        {
            get { return _responseObject; }
            set { _responseObject = value; }
        }




    }
}