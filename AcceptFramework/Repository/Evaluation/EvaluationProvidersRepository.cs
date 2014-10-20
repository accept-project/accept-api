using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationProvidersRepository : RepositoryBase<EvaluationProvider>
    {
        public static IEnumerable<EvaluationProvider> GetAll()
        {
            return new RepositoryBase<EvaluationProvider>().Select();
        }

        public static EvaluationProvider Insert(EvaluationProvider record)
        {
            return new RepositoryBase<EvaluationProvider>().Create(record);
        }

        public static EvaluationProvider Get(int id)
        {
            return new RepositoryBase<EvaluationProvider>().Select(a => a.Id == id).FirstOrDefault();

        }

        public static EvaluationProvider Update(EvaluationProvider provider)
        {
            return new RepositoryBase<EvaluationProvider>().Update(provider);
        }

    
    }
}
