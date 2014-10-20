using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Repository.PostEdit
{
    internal class DocumentRepository
    {

        public static IEnumerable<Document> GetAll()
        {
            return new RepositoryBase<Document>().Select();
        }

        public static Document Insert(Document record)
        {
            return new RepositoryBase<Document>().Create(record);
        }

        public static Document GetDocument(int documentId)
        {
            return new RepositoryBase<Document>().Select(a => a.Id == documentId).FirstOrDefault();

        }

        public static Document UpdateDocument(Document document)
        {
            return new RepositoryBase<Document>().Update(document);
        }

        public static Document GetDocumentByTextId(string textId)
        {
            return new RepositoryBase<Document>().Select(a => a.text_id == textId).FirstOrDefault();
        }

        public static IEnumerable<Document> GetAllByProjectId(Project p)
        {
            return new RepositoryBase<Document>().Select(a => a.Project == p);
        }

        public static void DeleteDocument(Document document)
        {
              new RepositoryBase<Document>().Delete(document);
        }
    }
}
