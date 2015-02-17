using System;
using System.Collections.Generic;
using System.ComponentModel;
using AcceptFramework.Domain.Common;
using AcceptFramework.Domain.PostEdit;
using AcceptPortal.Models.Evaluation;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationProject : DomainBase
    {
        public virtual string Name { get; set; }
        public virtual string Organization { get; set; }
        public virtual string Description { get; set; }
        public virtual string ApiKey { get; set; }
        public virtual string Domain { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual User Creator { get; set; }
        public virtual EvaluationStatus Status { get; set; }
        public virtual EvaluationType Type { get; set; }
        public virtual IList<EvaluationProvider> Providers { get; set; }
        public virtual IList<EvaluationLanguagePair> LanguagePairs { get; set; }
        public virtual IList<EvaluationQuestion> Questions { get; set; }
        public virtual EvaluationProjectMode Mode { get; set; }
        public virtual ICollection<EvaluationContentChunk> ContentChunks { get; set; }
        public virtual string AdminToken { get; set; }

        #region EvaluationInternal
        public virtual int PostEditProjectReferenceId { get; set; }
        public virtual Project PostEditProjectReference { get; set; }
        public virtual bool IncludePostEditProjectOwner { get; set; }
        public virtual InternalProjectEvaluationMethod PostEditProjectEvaluationMethod { get; set; }
        public virtual InternalProjectAvoidDuplicates PostEditProjectDuplicationLogic { get; set; }
        public virtual string EmailBodyMessage { get; set; }


        public EvaluationProject()
        {
            Providers = new List<EvaluationProvider>();
            Questions = new List<EvaluationQuestion>();
            LanguagePairs = new List<EvaluationLanguagePair>();
            Status = EvaluationStatus.InProgress;
            Mode = EvaluationProjectMode.Source | EvaluationProjectMode.Bilingual | EvaluationProjectMode.Target;
            CreationDate = DateTime.MinValue;
            this.ContentChunks = new List<EvaluationContentChunk>();
            this.AdminToken = string.Empty;

            this.PostEditProjectReferenceId = -1;
            this.PostEditProjectReference = null;
            this.PostEditProjectEvaluationMethod = InternalProjectEvaluationMethod.NotSet;
            this.PostEditProjectDuplicationLogic = InternalProjectAvoidDuplicates.No;
            this.EmailBodyMessage = string.Empty;
            this.IncludePostEditProjectOwner = false;

        }

        public EvaluationProject(string name, EvaluationType type)
        {
            Name = name;
            Type = type;
            this.AdminToken = string.Empty;
            this.ContentChunks = new List<EvaluationContentChunk>();
            this.PostEditProjectReferenceId = -1;
            this.PostEditProjectReference = null;
            this.PostEditProjectEvaluationMethod = InternalProjectEvaluationMethod.NotSet;
            this.PostEditProjectDuplicationLogic = InternalProjectAvoidDuplicates.No;
            this.EmailBodyMessage = string.Empty;

        }

        public enum InternalProjectEvaluationMethod : int
        {
            [Description("Not Set")]
            NotSet = 0,
            [Description("Evaluate MT")]
            EvaluateOriginalOnly = 1,
            [Description("Evaluate MT and Revisions")]
            EvaluateOriginalAndAllRevisions = 2,
            [Description("Evaluate Only Revisions")]
            EvaluateOnlyRevisions = 3
            //evaluationMethods.Add(new SelectListItem() { Text = Global.EvaluateOriginalLabel, Value = "1" });
            //evaluationMethods.Add(new SelectListItem() { Text = Global.EvaluateOriginalAndAllRevisionsLabel, Value = "2" });
            //evaluationMethods.Add(new SelectListItem() { Text = Global.EvaluateAllRevisionsOnlyLabel, Value = "3" });            
        }

        public enum InternalProjectAvoidDuplicates : int
        {
            [Description("No")]
            No = 0,

            [Description("Yes, Task Level")]
            YesTaskLevel = 1,

            [Description("Yes, Project Level")]
            YesProjectLevel = 2,
        }

        #endregion

    }
}