using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Core
{
   
    public class AcceptApiResponseManager
    {
        private AcceptApiResponseStatus _responseStatus;
        private AcceptApiResponse _responseObject;  
        
        private string _jsonRaw;          
        private List<ResultSet> _resultSet;
      
        public AcceptApiResponseManager()
        {
            _jsonRaw = string.Empty;
            _responseObject = new AcceptApiResponse();
            _resultSet = new List<ResultSet>();
            _responseStatus = null;
        }     

        #region Properties

        public AcceptApiResponseStatus ResponseStatus
        {
            get { return _responseStatus; }
            set { _responseStatus = value; }
        }               
        
        public string JsonRaw
        {
            get { return _jsonRaw; }
            set { _jsonRaw = value; }
        }
        
        public List<ResultSet> ResultSet
        {
            get { return _resultSet; }
            set { _resultSet = value; }
        }        
        
        public AcceptApiResponse ResponseObject
        {
            get { return _responseObject; }
            set { _responseObject = value; }
        }

        public void AppendResultSetList(List<ResultSet> resultSet)
        {
            this.ResultSet.Concat(resultSet);
        }

        public void MarkResultSetComplete()
        {
            this.ResponseObject.Response.ResultSet = _resultSet.ToArray();
        }

        #endregion               
    }
   
}