using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.PostEdit
{
    public class ProjectRequestObject
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int domainId { get; set; }
        public int status { get; set; }
        public List<string> projectQuestions { get; set; }
        public  int sourceLangId { get; set; }
        public  int targetLangId { get; set; }
        public  int interfaceConfig { get; set; }
        public  string projectOwner { get; set; }
        public  DateTime creationDate { get; set; }
        public List<string> projectOptions { get; set; }
        public int translationOptions { get; set; }
        public string emailMessage { get; set; }
        public string surveyLink { get; set; }
        public string projectOrganization { get; set; }
        public int customInterfaceConfiguration { get; set; }
        public bool isExternalProject { get; set; }

        public bool isSingleRevision { get; set; }
        public string maxThreshold { get; set; }

        public int paraphrasingMode { get; set; }
        public int interactiveCheck { get; set; }
        public string interactiveCheckMetadata { get; set; }
        public string paraphrasingMetadata { get; set; }

        public ProjectRequestObject()
        {
            this.ID = -1;
            this.name = string.Empty;
            this.domainId = -1;
            this.status = -1;
            this.projectQuestions = new List<string>();
            this.projectOptions = new List<string>();
            this.sourceLangId = -1;
            this.targetLangId = -1;
            this.interfaceConfig = -1;
            this.projectOwner = string.Empty;
            this.creationDate = DateTime.UtcNow;
            this.translationOptions = 0;
            this.emailMessage = string.Empty;
            this.surveyLink = string.Empty;
            this.projectOrganization = string.Empty;
            this.customInterfaceConfiguration = 0;
            this.isExternalProject = false;
            this.isSingleRevision = false;
            this.maxThreshold = string.Empty;

            this.paraphrasingMode = 0;
            this.interactiveCheck = 0;
            this.interactiveCheckMetadata = null;
            this.paraphrasingMetadata = null;


        }
    }
}