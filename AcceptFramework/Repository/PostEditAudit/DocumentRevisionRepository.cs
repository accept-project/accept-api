using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Repository.PostEditAudit
{
    internal class DocumentRevisionRepository
    {

        public static IEnumerable<DocumentRevision> GetAll()
        {
            return new RepositoryBase<DocumentRevision>().Select();
        }

        public static DocumentRevision Insert(DocumentRevision record)
        {
            return new RepositoryBase<DocumentRevision>().Create(record);
        }


        public static DocumentRevision GetDocumentRevision(int documentRevisionId)
        {
            return new RepositoryBase<DocumentRevision>().Select(a => a.Id == documentRevisionId).FirstOrDefault();
        }

        public static DocumentRevision GetDocumentRevisionByUserIdentifier(string documentId, string userIdentifier)
        {
            return new RepositoryBase<DocumentRevision>().Select(a => a.UserIdentifier == userIdentifier && a.DocumentId == documentId).FirstOrDefault();
        }

        public static IEnumerable<DocumentRevision> GetAllDocumentRevisionBytexttId(string documentId)
        {
            return new RepositoryBase<DocumentRevision>().Select(a => a.DocumentId == documentId);
        }


        public static IEnumerable<DocumentRevision> GetAllDocumentRevisionByUserIdentifier(string userIdentifier)
        {
            return new RepositoryBase<DocumentRevision>().Select(a => a.UserIdentifier == userIdentifier);
        }

        public static DocumentRevision UpdateDocumentRevision(DocumentRevision documentRevision)
        {
            return new RepositoryBase<DocumentRevision>().Update(documentRevision);
        }


        public static DocumentRevision GetDocumentRevisionByUserIdentifierWithUpdateNoWaitLock(string documentId, string userIdentifier)
        {
            return new RepositoryBase<DocumentRevision>().GetWithLock(a => a.UserIdentifier == userIdentifier && a.DocumentId == documentId, NHibernate.LockMode.Upgrade);
        }

        public static DocumentRevision GetDocumentRevisionByTextId(string textId)
        {
            return new RepositoryBase<DocumentRevision>().Select(a => a.DocumentId == textId).FirstOrDefault();
        }

    }
}
