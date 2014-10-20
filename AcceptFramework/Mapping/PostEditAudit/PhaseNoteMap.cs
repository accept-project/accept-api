using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class PhaseNoteMap: ClassMap<PhaseNote>
    {

        public PhaseNoteMap()
        {

            Table("Notes");

            Id(e => e.Id);
            Map(e => e.Annotates).Length(250);
            Map(e => e.Note).Length(2500);
            Map(e => e.NoteFrom).Length(250);
            
                
        }
    
    
    
    }
}
