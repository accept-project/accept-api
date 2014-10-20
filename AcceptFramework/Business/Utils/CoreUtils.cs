using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Business.Utils
{
    public static class CoreUtils
    {
        public static double GetDifferenceInSeconds(DateTime start, DateTime end)
        {
            return (end - start).TotalSeconds;
        }
    
    }
}
