using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class PhaseNote:DomainBase
    {
        public virtual string Annotates {get;set;}
        public virtual string Note { get; set; }
        public virtual string NoteFrom { get; set; }

        public PhaseNote()
        { 
        
        }

        public PhaseNote(string annotates, string note, string noteFrom)
        {
               this.Annotates = annotates;
               this.Note =note;
               this.NoteFrom = noteFrom;
        }


    }
}
