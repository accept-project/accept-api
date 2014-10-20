using System;
using System.ComponentModel;
using System.Reflection;

namespace AcceptPortal.Models.Evaluation
{
    public enum EvaluationType: int
    {
        [Description("Embedded Evaluation")]
        Embedded = 1,

        [Description("External Evaluation")]
        External = 2,

    }

}