using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class ThinkPhase: Phase
    {
        public ThinkPhase()
        {
            Notes = new List<PhaseNote>();
            PhaseCountGroup = new PhaseCountGroup();        
        }


        public ThinkPhase(string phaseName, string processName, DateTime date, string jobId, string tool, string toolId, string contactEmail, List<PhaseNote> phaseNotes, PhaseCountGroup countGroup)
        {

            this.PhaseName = phaseName;
            this.ProcessName = processName;
            this.Date = date;
            this.JobId = jobId;
            this.Tool = tool;
            this.ToolId = toolId;
            this.ContactEmail = contactEmail;
            this.Notes = phaseNotes;
            this.PhaseCountGroup = countGroup;
        }
       
    
    }


}
