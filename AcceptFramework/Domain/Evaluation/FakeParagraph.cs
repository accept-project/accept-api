namespace AcceptFramework.Domain.Evaluation
{
    public class FakeParagraph : DomainBase
    {
        public int SegmentNumber { get; set; }
        public bool Completed { get; set; }
        public int CompletionTime { get; set; }
    }
}
