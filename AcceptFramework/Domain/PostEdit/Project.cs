using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Domain.PostEdit
{
    public class Project : DomainBase
    {
        public virtual string Name { get; set; }        
        public virtual int Status { get; set; }        
        public virtual AcceptFramework.Domain.Common.Domain ProjectDomain { get; set; }
        public virtual ICollection<Document> ProjectDocuments { get; set; }
        public virtual ICollection<ProjectQuestion> ProjectQuestion { get; set; }
        public virtual EvaluationLanguage SourceLanguage { get; set; }
        public virtual EvaluationLanguage TargetLanguage { get; set; }
        public virtual int InterfaceConfiguration { get; set; }
        public virtual string ProjectOwner { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual ICollection<PostEditOption> ProjectEditOptions { get; set; }
        public virtual int TranslationOptions { get; set; }
        public virtual string EmailBodyMessage { get; set; }
        public virtual string SurveyLink { get; set; }
        public virtual string ProjectOrganization { get; set; }
        public virtual int CustomInterfaceConfiguration { get; set; }
        public virtual bool External { get; set; }
        public virtual string AdminToken { get; set; }
        public virtual bool IsSingleRevision { get; set; }
        public virtual bool ResetBlockTimeStampWhenSameUser { get; set; }
        public virtual TimeSpan MaxThreshold { get; set; }
        public virtual TimeSpan MaxIdleThreshold { get; set; }
        public virtual int ParaphrasingMode { get; set; }
        public virtual int InteractiveCheck { get; set; }
        public virtual string InteractiveCheckMetadata { get; set; }
        public virtual string ParaphrasingMetadata { get; set; }

        public Project()
        {
            this.Name = string.Empty; 
            this.ProjectDomain = null; 
            this.Status = 1; 
            this.ProjectDocuments = new List<Document>();
            this.ProjectQuestion = new List<ProjectQuestion>(); 
            this.SourceLanguage = new EvaluationLanguage();
            this.TargetLanguage = new EvaluationLanguage();
            this.InterfaceConfiguration = -1; 
            this.ProjectOwner = string.Empty; 
            this.ProjectEditOptions = new List<PostEditOption>();
            this.CreationDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            this.TranslationOptions = 0;
            this.EmailBodyMessage = string.Empty;
            this.SurveyLink = string.Empty;
            this.ProjectOrganization = string.Empty;
            this.CustomInterfaceConfiguration = 0;
            this.External = false;
            this.AdminToken = string.Empty;
            this.IsSingleRevision = false;
            this.MaxIdleThreshold = new TimeSpan();
            this.MaxThreshold = new TimeSpan();
            this.ResetBlockTimeStampWhenSameUser = false;
            this.ParaphrasingMode = 0;
            this.InteractiveCheck = 0;
            this.InteractiveCheckMetadata = string.Empty;
            this.ParaphrasingMetadata = string.Empty;
        }
       
        public Project(string name, AcceptFramework.Domain.Common.Domain domain, int status, List<Document> documents, List<ProjectQuestion> projectQuestions, EvaluationLanguage sourceLanguage, EvaluationLanguage targetLanguage, string projectOwner, int interfaceConfiguration, DateTime creationDate, List<PostEditOption> editOptions, int translationOptions, string emailBodyText, string surveyLink, string projectOrganization, int customInterfaceConfig, bool isExternalProject, string adminToken,bool isSingleRevision, TimeSpan maxThre, int paraphrasingMode, int interactiveCheck,string interactiveCheckMetadata,
            string paraphrasingMetadata) 
        {
            this.Name = name;
            this.ProjectDomain = domain;
            this.Status = status;
            this.ProjectDocuments = documents;
            this.ProjectQuestion = projectQuestions;
            this.SourceLanguage = sourceLanguage;
            this.TargetLanguage = targetLanguage;
            this.ProjectOwner = projectOwner; 
            this.InterfaceConfiguration = interfaceConfiguration;
            this.CreationDate = creationDate;
            this.ProjectEditOptions = editOptions;
            this.TranslationOptions = translationOptions;
            this.EmailBodyMessage = emailBodyText;
            this.SurveyLink = surveyLink;
            this.ProjectOrganization = projectOrganization;
            this.CustomInterfaceConfiguration = customInterfaceConfig;
            this.External = isExternalProject;
            this.AdminToken = adminToken;
            this.MaxIdleThreshold = maxThre;
            this.MaxThreshold = maxThre;
            this.IsSingleRevision = isSingleRevision;
            this.ParaphrasingMode = paraphrasingMode;
            this.InteractiveCheck = interactiveCheck;
            this.InteractiveCheckMetadata = interactiveCheckMetadata;
            this.ParaphrasingMetadata = paraphrasingMetadata;
        }

        public override void Validate()
        {
            base.Validate();

            if (this.SurveyLink == null)
                this.SurveyLink = string.Empty;

            if (this.ProjectOrganization == null)
                this.ProjectOrganization = string.Empty;

            if (this.EmailBodyMessage == null)
                this.EmailBodyMessage = string.Empty;
        }               
    }
}
