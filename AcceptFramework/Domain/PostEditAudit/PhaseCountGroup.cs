using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class PhaseCountGroup: DomainBase
    {
        public virtual string Name { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual ICollection<PhaseCount> PhaseCounts { get; private set; }


        public PhaseCountGroup()
        {
            PhaseCounts = new List<PhaseCount>();
        }


        public PhaseCountGroup(string name, List<PhaseCount> phaseGroupCounts, DateTime dateAdded)
        {
            this.Name = name;
            this.DateAdded = dateAdded;
            this.PhaseCounts = phaseGroupCounts;
        }

    }
}
