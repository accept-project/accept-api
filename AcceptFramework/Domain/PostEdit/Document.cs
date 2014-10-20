using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;


namespace AcceptFramework.Domain.PostEdit
{
    public class Document: DomainBase
    {

        public virtual string text_id { get; set; }
        public virtual string src_text { get; set; }
        public virtual string tgt_text { get; set; }
        public virtual ICollection<TargetSentence> tgt_sentences { get; set; }
        public virtual ICollection<SourceSentence> src_sentences { get; set; }      
        public virtual Project Project { get; set; }

        #region Metadata       
        public virtual string Original { get; set; }        
        public virtual string SourceLanguage { get; set; }       
        public virtual string TargetLanguage { get; set; }
        public virtual string DataType { get; set; }
        public virtual string Category { get; set; }
        public virtual string ProductName { get; set; }       
        public virtual string MtTool { get; set; }
        public virtual string MtToolId { get; set; }
        public virtual string MtDate { get; set; }
        public virtual string MtContactEmail { get; set; }              
        #endregion
        
        public virtual ICollection<DocumentRevision> DocumentRevisions { get; set; }                       
        public virtual ICollection<TargetTemplate> tgt_templates { get; set; }
        public virtual bool IsSingleRevision { get; set; }
        public virtual string UniqueReviewerId { get; set; }

        public Document()        
        {
            this.text_id = string.Empty;
            this.src_text = string.Empty;
            this.tgt_text = string.Empty;
            this.tgt_sentences = new List<TargetSentence>();
            this.src_sentences = new List<SourceSentence>();
            this.tgt_templates = new List<TargetTemplate>();
            #region Metadata                     
            this.DocumentRevisions = new List<DocumentRevision>();
            this.Original = string.Empty;
            this.ProductName = string.Empty;
            this.SourceLanguage = string.Empty;
            this.DataType = string.Empty;
            this.Category = string.Empty;           
            this.TargetLanguage = string.Empty;
            this.Project = new Project();
            this.MtTool = string.Empty;
            this.MtContactEmail = string.Empty;
            this.MtToolId =string.Empty;
            this.MtDate = string.Empty;
            this.IsSingleRevision = false;
            this.UniqueReviewerId = null;
            #endregion                            
        }

        public Document(string text_id, string src_text, string tgt_text, List<TargetSentence> tgt_sentences, List<string> src_sentences, List<DocumentRevision> documentRevisions, string original, string productName, string dataType, string category, string sourceLang, string targetLang, Project project, List<TargetTemplate> tgt_templates, bool isSingleRevision, string uniqueReviewer)
        {
            this.text_id = string.Empty;
            this.src_text = string.Empty;
            this.tgt_text = string.Empty;
            this.tgt_sentences = new List<TargetSentence>();
            this.src_sentences = new List<SourceSentence>();
            this.tgt_templates = tgt_templates;            
            #region Metadata
            this.DocumentRevisions = documentRevisions;
            this.Original = original;
            this.ProductName = productName;
            this.SourceLanguage = sourceLang;
            this.DataType = dataType;
            this.Category = category;
            this.TargetLanguage = targetLang;
            this.Project = project;
            this.IsSingleRevision = isSingleRevision;
            this.UniqueReviewerId = uniqueReviewer;
            #endregion
        }
            
        public override void Validate()
        {
            base.Validate();


            if (this.Category == null)
                this.Category = string.Empty;

            if (this.Original == null)
                this.Original = string.Empty;

            if (this.ProductName == null)
                this.ProductName = string.Empty;

            if (this.SourceLanguage == null)
                this.SourceLanguage = string.Empty;

            if (this.DataType == null)
                this.DataType = string.Empty;

            if (this.TargetLanguage == null)
                this.TargetLanguage = string.Empty;

            if (this.MtTool == null)
                this.MtTool = string.Empty;

            if (this.MtContactEmail == null)
                this.MtContactEmail = string.Empty;

            if (this.MtToolId == null)
                this.MtToolId = string.Empty;

            if (this.MtDate == null)
                this.MtDate = string.Empty;          
        }

    }
    
    
   
}
