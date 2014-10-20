using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Repository.PostEditAudit
{
    internal class PhaseRepository
    {

        public static IEnumerable<Phase> GetAll()
        {
            return new RepositoryBase<Phase>().Select();
        }

        public static Phase Insert(Phase record)
        {
            return new RepositoryBase<Phase>().Create(record);
        }


        public static Phase GetPhase(int phaseId)
        {
            return new RepositoryBase<Phase>().Select(a => a.Id == phaseId).FirstOrDefault();
        }

        public static Phase UpdatePhase(Phase phase)
        {
            return new RepositoryBase<Phase>().Update(phase);
        }
                 
    }
}
