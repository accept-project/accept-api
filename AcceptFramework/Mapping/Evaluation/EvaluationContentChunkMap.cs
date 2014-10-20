using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationContentChunkMap: ClassMap<EvaluationContentChunk>
    {
        public EvaluationContentChunkMap()
        {
            Table("EvaluationContentChunks");
            Id(e => e.Id);
            Map(e => e.Chunk).CustomSqlType("ntext").Length(1073741823);       
            Map(e => e.ChunkInfo).Length(2500);            
            Map(e => e.Type);
            Map(e => e.Status);
        
        }
    
    }
}
