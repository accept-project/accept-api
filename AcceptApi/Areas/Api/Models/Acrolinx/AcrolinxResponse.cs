using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

using AcceptApi.Areas.Api.Models.Core;

namespace AcceptApi.Areas.Api.Models.Acrolinx
{

    [Serializable]
    [DataContract]
    public class AcrolinxResponse
    {

        [DataMember(Name = "checkReportXmlUrl", IsRequired = false)]
        public string CheckReportXmlUrl { get; set; }

        [DataMember(Name = "checkReportJsonUrl", IsRequired = false)]
        public string CheckReportJsonUrl { get; set; }

        [DataMember(Name = "annotatedInputXmlUrl", IsRequired = false)]
        public string AnnotatedInputXmlUrl { get; set; }

        [DataMember(Name = "termHarvestingOlifUrl", IsRequired = false)]
        public string TermHarvestingOlifUrl { get; set; }


        [DataMember(Name = "resultDetails", IsRequired = false)]
        public ResultDetails ResultDetails { get; set; }

        
        [DataMember(Name = "languageServerInstance", IsRequired = false)]
        public string LanguageServerInstance { get; set; }
         
    
    }


    [Serializable]
    [DataContract]
    public class ResultDetails
    {

        [DataMember(Name = "documentScore", IsRequired = false)]
        public string DocumentScore { get; set; }

        [DataMember(Name = "documentStatus", IsRequired = false)]
        public string DocumentStatus { get; set; }

        [DataMember(Name = "documentFlagCount", IsRequired = false)]
        public string DocumentFlagCount { get; set; }

        [DataMember(Name = "sentenceCount", IsRequired = false)]
        public string SentenceCount { get; set; }

        [DataMember(Name = "resultFlagTypes", IsRequired = false)]
        public string[] ResultFlagTypes { get; set; }

        [DataMember(Name = "resultFlagCountsByType", IsRequired = false)]
        public string[] ResultFlagCountsByType { get; set; }
       
    }



}