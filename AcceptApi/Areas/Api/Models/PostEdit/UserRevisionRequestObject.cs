using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptApi.Areas.Api.Models.PostEdit
{
    public class UserRevisionRequestObject
    {
        #region globalInfo
        
        public string textId { get; set; }
        public string userIdentifier { get; set; }

        #endregion

        #region TranslationUnitInfo

        public int index { get; set; }
        public string state { get; set; }
        public string source { get; set; }
        public string target { get; set; }

        #endregion

        #region Phases
        
        public List<PhaseRequestObject> phaseList { get; set; }

        #endregion

        #region PhaseCounts

        public string countGroupName { get; set; }
        public List<PhaseCount> phaseCounts {get;set;}
        
        #endregion


    }
}