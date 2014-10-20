using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Repository.PostEditAudit
{
    internal class CountGroupRepository
    {

        public static IEnumerable<PhaseCountGroup> GetAll()
        {
            return new RepositoryBase<PhaseCountGroup>().Select();
        }

        public static PhaseCountGroup Insert(PhaseCountGroup record)
        {
            return new RepositoryBase<PhaseCountGroup>().Create(record);
        }


        public static PhaseCountGroup GetPhaseCountGroup(int phaseCountGroupId)
        {
            return new RepositoryBase<PhaseCountGroup>().Select(a => a.Id == phaseCountGroupId).FirstOrDefault();
        }

        public static PhaseCountGroup UpdatePhaseCountGroup(PhaseCountGroup phaseCountGroup)
        {
            return new RepositoryBase<PhaseCountGroup>().Update(phaseCountGroup);
        }


    
    }
}
