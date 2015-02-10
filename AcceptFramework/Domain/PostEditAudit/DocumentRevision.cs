using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class DocumentRevision: DomainBase
    {
        public virtual string UserIdentifier { get; set; }
        public virtual string DocumentId { get; set; }
        public virtual int Status { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateLastUpdate { get; set; }
        public virtual DateTime CompleteDate { get; set; }
        public virtual string RevisionHash { get; set; }
        public virtual ICollection<TranslationRevision> TranslationUnits { get; set; }        
        public virtual ICollection<QuestionReply> QuestionsReplied { get; set; }

        public virtual bool IsLocked { get; set; }
        public virtual string LockedBy { get; set; }
        public virtual DateTime ? LockedDate { get; set; }

        public DocumentRevision()
        {
           this.TranslationUnits = new List<TranslationRevision>();
           this.QuestionsReplied = new List<QuestionReply>();
           this.IsLocked = false;          
           this.UserIdentifier = string.Empty;
           this.DocumentId = string.Empty;
           this.Status = 0;
         
        }

        public DocumentRevision(string userId, string documentId, int status, DateTime dateCreated, DateTime dateLastUpdate, DateTime completeDate, string revisionHash, List<TranslationRevision> translationUnitRevision)
        {
            this.UserIdentifier = userId;
            this.DocumentId = documentId;
            this.DateCreated = dateCreated;
            this.DateLastUpdate = dateLastUpdate;
            this.CompleteDate = completeDate;
            this.RevisionHash = revisionHash;
            this.Status = status;
            this.TranslationUnits = translationUnitRevision;
            this.QuestionsReplied = new List<QuestionReply>();
            this.IsLocked = false;            
        }

        public DocumentRevision(string userId, string documentId, int status, DateTime dateCreated, DateTime dateLastUpdate, DateTime completeDate, string revisionHash, List<TranslationRevision> translationUnitRevision, List<QuestionReply> questionsReplied)
        {
            this.UserIdentifier = userId;
            this.DocumentId = documentId;
            this.DateCreated = dateCreated;
            this.DateLastUpdate = dateLastUpdate;
            this.CompleteDate = completeDate;
            this.RevisionHash = revisionHash;
            this.Status = status;
            this.TranslationUnits = translationUnitRevision;
            this.QuestionsReplied = questionsReplied;
            this.IsLocked = false;            
        }    
    }
}
