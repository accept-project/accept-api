using System;

namespace AcceptFramework.Domain.Evaluation
{
    public class FakeTask : DomainBase
    {
        public int UserID { get; set; }
        public string ProjectName { get; set; }
        public string SourceName { get; set; }
        public int DocumentID { get; set; }
        public DateTime CompletionDate { get; set; }
        public int CompletionTime { get; set; }
    }
}
