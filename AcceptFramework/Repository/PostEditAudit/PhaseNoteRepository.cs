using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Repository.PostEditAudit
{
    internal class PhaseNoteRepository
    {

        public static IEnumerable<PhaseNote> GetAll()
        {
            return new RepositoryBase<PhaseNote>().Select();
        }

        public static PhaseNote Insert(PhaseNote record)
        {
            return new RepositoryBase<PhaseNote>().Create(record);
        }


        public static PhaseNote GetPhaseNote(int phaseNoteId)
        {
            return new RepositoryBase<PhaseNote>().Select(a => a.Id == phaseNoteId).FirstOrDefault();
        }

        public static PhaseNote UpdatePhaseNote(PhaseNote phaseNote)
        {
            return new RepositoryBase<PhaseNote>().Update(phaseNote);
        }
    
            
    
    }
}
