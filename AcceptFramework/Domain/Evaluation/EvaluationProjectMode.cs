using System;

namespace AcceptFramework.Domain.Evaluation
{
    [Flags]
    public enum EvaluationProjectMode
    {
        None = 0,
        Source = 1,
        Target = 2,
        Bilingual = 4
    }
}