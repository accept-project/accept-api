using System;
using System.Collections.Generic;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationDocumentSimple
    {
        public string Name { get; set; }
        public string Provider { get; set; }
        public int ProviderId { get; set; }
        public string LanguagePair { get; set; }
        public int LanguagePairId { get; set; }

        public EvaluationDocumentSimple()
        {
        }
    }

}