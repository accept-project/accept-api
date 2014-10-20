using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.Acrolinx
{
    public static class AcrolinxResponseStatusTypes
    {
        public const string Done = "DONE";
        public const string Waiting = "WAITING";
        public const string Failed = "FAILED";
        public const string RunningPostProcessing = "RUNNING_POSTPROCESSING";
        public const string RunningProcessing = "RUNNING_PROCESSING";
        public const string RunningPreProcessing = "RUNNING_PREPROCESSING";
        public const string NotSet = "NOTSET"; 
    }
}