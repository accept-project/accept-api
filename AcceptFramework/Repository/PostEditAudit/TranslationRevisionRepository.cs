using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Repository.PostEditAudit
{
    internal class TranslationRevisionRepository
    {

        public static IEnumerable<TranslationRevision> GetAll()
        {
            return new RepositoryBase<TranslationRevision>().Select();
        }

        public static TranslationRevision Insert(TranslationRevision record)
        {
            return new RepositoryBase<TranslationRevision>().Create(record);
        }


        public static TranslationRevision GetTranslationRevision(int translationRevisionId)
        {
            return new RepositoryBase<TranslationRevision>().Select(a => a.Id == translationRevisionId).FirstOrDefault();
        }


        public static TranslationRevision UpdateTranslationRevision(TranslationRevision translationRevision)
        {
            return new RepositoryBase<TranslationRevision>().Update(translationRevision);
        }
    
    
    }
}
