using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationContentChunk: DomainBase
    {
        public virtual string Chunk { get; set; }
        public virtual int Status { get; set; }
        public virtual int Type { get; set; }
        public virtual string ChunkInfo { get; set; }

        public EvaluationContentChunk()
        {
            this.Chunk = string.Empty;
            this.Status = 1;
            this.Type = 1;
            this.ChunkInfo = string.Empty;        
        }
    
    }
}
