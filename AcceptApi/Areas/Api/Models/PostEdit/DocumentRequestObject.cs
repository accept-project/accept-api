using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptFramework.Domain.PostEdit;

namespace AcceptApi.Areas.Api.Models.PostEdit
{
    public class DocumentRequestObject
    {

        public virtual string text_id { get; set; }
        public virtual string src_text { get; set; }
        public virtual string tgt_text { get; set; }
        public virtual ICollection<TargetSentence> tgt_sentences { get; set; }      
        public virtual ICollection<SourceSentence> src_sentences { get; set; }
        public virtual ICollection<TargetTemplate> tgt_templates { get; set; }
        public virtual string original { get; set; }
        public virtual string sourceLanguage { get; set; }
        public virtual string targetLanguage { get; set; }
        public virtual string dataType { get; set; }
        public virtual string category { get; set; }
        public virtual string productName { get; set; }
        public virtual string questionIdentifier { get; set; }
        public virtual string projectQuestion { get; set; }
        public virtual string configurationId { get; set; }
        public virtual List<string> editOptions { get; set; }
        public virtual int displayTranslationOptions { get; set; }
        public virtual int customInterface { get; set; }
        public virtual List<List<string>> targetRevisions { get; set; }
        public virtual bool isSingleRevisionProject { get; set; }
        public virtual string maxThreshold { get; set; }
        public virtual string lastBlockDate { get; set; }
        public virtual string interactiveCheckMeta { get; set; }
        public virtual string paraphrasingServiceMeta { get; set; }
        public virtual int interactiveCheck { get; set; }
        public virtual int paraphrasingService { get; set; }

        public DocumentRequestObject()        
        {
            text_id = string.Empty;
            src_text = string.Empty;
            tgt_text = string.Empty;

            tgt_sentences = new List<TargetSentence>();
            src_sentences = new List<SourceSentence>();
            tgt_templates = new List<TargetTemplate>();
                       
            this.original = string.Empty;
            this.productName = string.Empty;
            this.sourceLanguage = string.Empty;
            this.dataType = string.Empty;
            this.category = string.Empty;           
            this.targetLanguage = string.Empty;
            this.questionIdentifier = string.Empty;
            this.projectQuestion = string.Empty;
            this.configurationId = string.Empty;
            this.editOptions = new List<string>();
            this.displayTranslationOptions = 1;
            this.customInterface = 0;
            this.targetRevisions = new List<List<string>>();

            this.isSingleRevisionProject = false;
            this.maxThreshold = string.Empty;
            this.lastBlockDate = string.Empty;

            this.interactiveCheckMeta = string.Empty;
            this.paraphrasingServiceMeta = string.Empty;
            this.interactiveCheck = 0;
            this.paraphrasingService = 0;

        }
    
    }
}