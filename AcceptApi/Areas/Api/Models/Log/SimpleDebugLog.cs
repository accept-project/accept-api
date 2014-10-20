using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.Log
{
    public static class SimpleDebugLog
    {

        public static string GetTempPath()
        {
            //string path = System.Environment.GetEnvironmentVariable("TEMP");
            //if (!path.EndsWith("\\")) path += "\\";
            //return path;
            return "C:\\Logs\\";
        }

        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
            GetTempPath() + "ACCEPT_API_Log.txt");
            try
            {
                string logLine = System.String.Format("{0:G}: {1}.", System.DateTime.UtcNow, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }

    }
}