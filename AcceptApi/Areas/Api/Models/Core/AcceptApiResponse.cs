using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Core
{
    [Serializable]
    [DataContract]
    public class AcceptApiResponse: CoreApiResponse
    {
        [DataMember(Name = "response", IsRequired = false)]
        public Response Response { get; set; }

        public AcceptApiResponse()
        {
            Response = new Response();
        }
    }

    [Serializable]
    [DataContract]
    public class Response
    {
        [DataMember(Name = "resultset", IsRequired = false)]
        public ResultSet[] ResultSet { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ResultSet
    {
        [DataMember(Name = "result", IsRequired = false)]
        public Result[] Results { get; set; }
    }


    [Serializable]
    [DataContract]
    public class Result
    {
        public Result()
        {
            Header = new ResponseHeader();
            Body = new ResponseBody();
        }

        [DataMember(Name = "header", IsRequired = false, Order = 1)]
        public ResponseHeader Header { get; set; }

        [DataMember(Name = "body", IsRequired = false, Order = 2)]
        public ResponseBody Body { get; set; }

    }


    [Serializable]
    [DataContract]
    public class ResponseHeader
    {
        public ResponseHeader()
        {
            this.Type = string.Empty;
            this.Description = string.Empty;
            this.Rule = string.Empty;
            this.ContextType = -1;
        }

        [DataMember(Name = "type", IsRequired = false, Order = 1)]
        public string Type { get; set; }

        [DataMember(Name = "description", IsRequired = false, Order = 2)]
        public string Description { get; set; }

        [DataMember(Name = "rule", IsRequired = false, Order = 3)]
        public string Rule { get; set; }

        [DataMember(Name = "contexttype", IsRequired = false, Order = 4)]
        public int ContextType { get; set; }

        [DataMember(Name = "uniqueId", IsRequired = false, Order = 5)]
        public string UniqueId { get; set; }

    }
    
    [Serializable]
    [DataContract]
    public class ResponseBody
    {
        public ResponseBody()
        {
            Context = string.Empty;
            ContextPieces = new ContextPiece[] { };
            StartPos = -1;
            EndPos = -1;
            Suggestion = null;
        }


        [DataMember(Name = "context", IsRequired = false, Order = 1)]
        public string Context { get; set; }

        [DataMember(Name = "startpos", IsRequired = false, Order = 2)]
        public int StartPos { get; set; }

        [DataMember(Name = "endpos", IsRequired = false, Order = 3)]
        public int EndPos { get; set; }

        [DataMember(Name = "contextpieces", IsRequired = false, Order = 4)]
        public ContextPiece[] ContextPieces { get; set; }

        [DataMember(Name = "suggestions", IsRequired = false, Order = 5)]
        public string[] Suggestion { get; set; }

    }


    [Serializable]
    [DataContract]
    public class ContextPiece
    {
        public ContextPiece(string piece, int starp, int endp)
        {
            Piece = piece;
            StartPos = starp;
            EndPos = endp;
        }

        public ContextPiece()
        {
            Piece = string.Empty;
            StartPos = -1;
            EndPos = -1;
        }

        [DataMember(Name = "piece", IsRequired = false, Order = 1)]
        public string Piece;
        [DataMember(Name = "startpos", IsRequired = false, Order = 2)]
        public int StartPos;
        [DataMember(Name = "endpos", IsRequired = false, Order = 3)]
        public int EndPos;
    }








}