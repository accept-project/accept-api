using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Repository.Common
{
    internal static class LanguageRepository
    {

        public static IEnumerable<AcceptFramework.Domain.Common.Language> GetAll()
        {
            return new RepositoryBase<AcceptFramework.Domain.Common.Language>().Select();
        }

        public static AcceptFramework.Domain.Common.Language Insert(AcceptFramework.Domain.Common.Language record)
        {
            return new RepositoryBase<AcceptFramework.Domain.Common.Language>().Create(record);
        }


        public static AcceptFramework.Domain.Common.Language GetLanguage(int languageId)
        {
            return new RepositoryBase<AcceptFramework.Domain.Common.Language>().Select(a => a.Id == languageId).FirstOrDefault();

        }

        public static AcceptFramework.Domain.Common.Language UpdateLanguage(AcceptFramework.Domain.Common.Language language)
        {
            return new RepositoryBase<AcceptFramework.Domain.Common.Language>().Update(language);
        }

    
    
    }
}
