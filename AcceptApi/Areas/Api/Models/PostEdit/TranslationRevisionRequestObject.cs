using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptFramework.Domain.PostEditAudit;
using System.Runtime.Serialization;

namespace AcceptApi.Areas.Api.Models.PostEdit
{
    [Serializable]
    [DataContract]
    public class TranslationRevisionRequestObject
    {
        [DataMember(Name = "userIdentifier", IsRequired = false)]
        public string userIdentifier { get; set; }
        [DataMember(Name = "textId", IsRequired = false)]
        public string textId { get; set; }
        [DataMember(Name = "index", IsRequired = false)]
        public string index { get; set; }
        [DataMember(Name = "timeStamp", IsRequired = false)]
        public string timeStamp { get; set; }
        [DataMember(Name = "state", IsRequired = false)]
        public string state { get; set; }
        [DataMember(Name = "source", IsRequired = false)]
        public string source { get; set; }
        [DataMember(Name = "target", IsRequired = false)]
        public string target { get; set; }

        [DataMember(Name = "phase", IsRequired = true)]
        public PhaseRequestObject phase { get; set; }      
        [DataMember(Name = "phaseCounts", IsRequired = true)]
        public List<PhaseCount> phaseCounts { get; set; }
        [DataMember(Name = "phaseDate", IsRequired = true)]
        public string phaseDate { get; set; }
        
        public TranslationRevisionRequestObject()
        {
            this.userIdentifier = string.Empty;
            this.textId = string.Empty;
            this.index = string.Empty;
            this.timeStamp = string.Empty;
            this.state = string.Empty;
            this.source = string.Empty;
            this.target = string.Empty;           
            this.phase = new PhaseRequestObject();
            this.phaseCounts = new List<PhaseCount>();
            this.phaseDate = string.Empty;                
        }        
    }

    [Serializable]
    [DataContract]
    public class PhaseRequestObject
    {
        [DataMember(Name = "PhaseName", IsRequired = false)]
        public  string PhaseName { get; set; }
        [DataMember(Name = "ProcessName", IsRequired = false)]
        public  string ProcessName { get; set; }
        [DataMember(Name = "Date", IsRequired = false)]
        public  DateTime? Date { get; set; }
        [DataMember(Name = "JobId", IsRequired = false)]
        public  string JobId { get; set; }
        [DataMember(Name = "Tool", IsRequired = false)]
        public  string Tool { get; set; }
        [DataMember(Name = "ToolId", IsRequired = false)]
        public  string ToolId { get; set; }
        [DataMember(Name = "ContactEmail", IsRequired = false)]
        public  string ContactEmail { get; set; }

        [DataMember(Name = "Notes", IsRequired = false)]
        public List<PhaseNotesRequestObject> Notes { get; private set; }


        public PhaseRequestObject()
        {
            this.PhaseName = string.Empty;
            this.ProcessName = string.Empty;
            this.Date = Date.GetValueOrDefault();
            this.JobId = string.Empty;
            this.Tool = string.Empty;
            this.ToolId = string.Empty;
            this.ContactEmail = string.Empty;
            this.Notes = new List<PhaseNotesRequestObject>();        
        }
    }

    [Serializable]
    [DataContract]
    public class PhaseNotesRequestObject
    {
         [DataMember(Name = "Annotates", IsRequired = false)]
        public  string Annotates { get; set; }
          [DataMember(Name = "Note", IsRequired = false)]
        public  string Note { get; set; }
          [DataMember(Name = "NoteFrom", IsRequired = false)]
        public  string NoteFrom { get; set; }


        public PhaseNotesRequestObject()
        {
            this.Annotates = string.Empty;
            this.Note = string.Empty;
            this.NoteFrom = string.Empty;
        
        }
    }


}