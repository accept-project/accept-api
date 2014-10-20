using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Repository.PostEditAudit
{
    internal class ThinkPhaseRepository
    {

        public static IEnumerable<ThinkPhase> GetAll()
        {
            return new RepositoryBase<ThinkPhase>().Select();
        }

        public static ThinkPhase Insert(ThinkPhase record)
        {
            return new RepositoryBase<ThinkPhase>().Create(record);
        }


        public static ThinkPhase GetPhase(int phaseId)
        {
            return new RepositoryBase<ThinkPhase>().Select(a => a.Id == phaseId).FirstOrDefault();
        }

        public static ThinkPhase UpdatePhase(ThinkPhase phase)
        {
            return new RepositoryBase<ThinkPhase>().Update(phase);
        }

    }
}
