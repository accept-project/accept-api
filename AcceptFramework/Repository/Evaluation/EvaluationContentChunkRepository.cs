using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationContentChunkRepository
    {

        public static EvaluationContentChunk Insert(EvaluationContentChunk record)
        {
            return new RepositoryBase<EvaluationContentChunk>().Create(record);
        }


        public static IEnumerable<EvaluationContentChunk> GetAllChunksByProjectId(int chunkId)
        {
            return new RepositoryBase<EvaluationContentChunk>().Select(a => a.Id == chunkId);
        }


        
    
    
    }



}
