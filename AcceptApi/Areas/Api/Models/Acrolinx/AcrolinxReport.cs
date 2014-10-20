using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.Acrolinx
{

    [Serializable]
    [DataContract]
    public class AcrolinxReport 
    {
        [DataMember(Name = "report", IsRequired = false)]
        public Report Report { get; set; }                                        
    }


    [Serializable]
    [DataContract]
    public class Report
    {
        public Report()
        {
            Flags = new Flag[] { };
        }
        [DataMember(Name = "flags", IsRequired = false)]
        public Flag[] Flags { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Flag
    {
        [DataMember(Name = "type", IsRequired = false)]
        public string Type { get; set; }

        [DataMember(Name = "description", IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "help", IsRequired = false)]
        public string Help { get; set; }

        [DataMember(Name = "suggestions", IsRequired = false)]
        public Sugestion[] Suggestions { get; set; }

        [DataMember(Name = "positionalInformation", IsRequired = false)]
        public PositionalInformation PositionalInformation { get; set; }

        [DataMember(Name = "termCandidate", IsRequired = false)]
        public TermCandidate TermCandidate { get; set; }

        [DataMember(Name = "key", IsRequired = false)]
        public string Key { get; set; }

        [DataMember(Name = "groupId", IsRequired = false)]
        public string GroupId { get; set; }

        [DataMember(Name = "flagId", IsRequired = false)]
        public string FlagId { get; set; }        
    }

    [Serializable]
    [DataContract]
    public class Sugestion
    {
        [DataMember(Name = "surface", IsRequired = false)]
        public string Surface { get; set; }

        [DataMember(Name = "groupId", IsRequired = false)]
        public string GroupId { get; set; }         
    }

    [Serializable]
    [DataContract]
    public class PositionalInformation
    {
        [DataMember(Name = "matches", IsRequired = false)]
        public Match[] Matches { get; set; }          
    }


    [Serializable]
    [DataContract]
    public class Match
    {
        [DataMember(Name = "part", IsRequired = false)]
        public string Part { get; set; }
        [DataMember(Name = "begin", IsRequired = false)]
        public int Begin { get; set; }
        [DataMember(Name = "end", IsRequired = false)]
        public int End { get; set; }
    }


    [Serializable]
    [DataContract]
    public class TermCandidate
    {
        [DataMember(Name = "lemma", IsRequired = false)]
        public string Lemma { get; set; }

        [DataMember(Name = "partOfSpeech", IsRequired = false)]
        public string PartOfSpeech { get; set; }

        [DataMember(Name = "termContributionUrl", IsRequired = false)]
        public string TermContributionUrl { get; set; }

    }







}
