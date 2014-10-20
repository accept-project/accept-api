using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptApi.Areas.Api.Models.PostEdit
{
    public class PhaseRequestObject
    {
       //string phaseName, string processName, DateTime date, string jobId, string contactEmail, string tool, string toolId, List<PhaseNote> phaseNotes

        public string phaseName { get; set; }
        public string processName { get; set; }
        public DateTime date { get; set; }
        public string jobId { get; set; }
        public string contactEmail { get; set; }
        public string tool { get; set; }
        public string toolId { get; set; }
        public List<PhaseNote> phaseNotes { get; set; }
     
    }
}