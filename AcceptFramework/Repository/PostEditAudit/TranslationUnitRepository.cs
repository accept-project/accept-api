using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Repository.PostEditAudit
{
    internal class TranslationUnitRepository
    {

        public static IEnumerable<TranslationUnit> GetAll()
        {
            return new RepositoryBase<TranslationUnit>().Select();
        }

        public static TranslationUnit Insert(TranslationUnit record)
        {
            return new RepositoryBase<TranslationUnit>().Create(record);
        }


        public static TranslationUnit GetTranslationUnit(int translationUnitId)
        {
            return new RepositoryBase<TranslationUnit>().Select(a => a.Id == translationUnitId).FirstOrDefault();
        }

        public static TranslationUnit UpdateTranslationUnit(TranslationUnit translationUnit)
        {
            return new RepositoryBase<TranslationUnit>().Update(translationUnit);
        }
    
    
    
    }
}
