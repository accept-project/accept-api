using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Repository.PostEditAudit
{
    internal class CountRepository
    {

        public static IEnumerable<PhaseCount> GetAll()
        {
            return new RepositoryBase<PhaseCount>().Select();
        }

        public static PhaseCount Insert(PhaseCount record)
        {
            return new RepositoryBase<PhaseCount>().Create(record);
        }


        public static PhaseCount GetPhaseCount(int phaseCountId)
        {
            return new RepositoryBase<PhaseCount>().Select(a => a.Id == phaseCountId).FirstOrDefault();
        }

        public static PhaseCount UpdatePhaseCount(PhaseCount phaseCount)
        {
            return new RepositoryBase<PhaseCount>().Update(phaseCount);
        }
    }
}
