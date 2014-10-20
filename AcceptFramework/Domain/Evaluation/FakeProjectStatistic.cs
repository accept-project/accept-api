namespace AcceptFramework.Domain.Evaluation
{
    public enum Direction{Both, Target, Source}

    public class FakeProjectStatistic : DomainBase
    {
        public string LanguageCombination { get; set; }
        public EvaluationDirection _Direction { get; set; }
        public string Provider { get; set; }
        public int Scorings { get; set; }
        public int Documents { get; set; }
        public double AvgCompScore { get; set; }
        public string AvgFidelity { get; set; }
    }
}
