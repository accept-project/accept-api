using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class Phase : DomainBase
    {

        public virtual string PhaseName { get; set; }
        public virtual string ProcessName { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string JobId { get; set; }
        public virtual string Tool { get; set; }
        public virtual string ToolId { get; set; }
        public virtual string ContactEmail { get; set; }

        public virtual ICollection<PhaseNote> Notes { get; set; }
        public virtual PhaseCountGroup PhaseCountGroup { get; set; }
    
        public Phase()
        {
            Notes = new List<PhaseNote>();
            PhaseCountGroup = new PhaseCountGroup();
        }


        public Phase(string phaseName, string processName, DateTime date, string jobId, string tool, string toolId, string contactEmail, List<PhaseNote> phaseNotes)
        {

            this.PhaseName = phaseName;
            this.ProcessName = processName;
            this.Date = date;
            this.JobId = jobId;
            this.Tool = tool;
            this.ToolId = toolId;
            this.ContactEmail = contactEmail;

            Notes = phaseNotes;

            PhaseCountGroup = new PhaseCountGroup();
        }


        public Phase(string phaseName, string processName, DateTime date, string jobId, string tool, string toolId, string contactEmail, List<PhaseNote> phaseNotes, PhaseCountGroup phaseCountGroup)
        {

            this.PhaseName = phaseName;
            this.ProcessName = processName;
            this.Date = date;
            this.JobId = jobId;
            this.Tool = tool;
            this.ToolId = toolId;
            this.ContactEmail = contactEmail;

            this.Notes = phaseNotes;
            this.PhaseCountGroup = phaseCountGroup;
        }
    
    }
}
