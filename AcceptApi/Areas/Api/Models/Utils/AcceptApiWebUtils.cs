using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;




namespace AcceptApi.Areas.Api.Models.Utils
{
     public static class AcceptApiWebUtils
    {

        /// <summary>
        /// Build Web Request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateRequest(string url)
        {
           
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            return request;
        }

        /// <summary>
        /// Build Web Request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateRequest(string url, WebHeaderCollection headers, string method, string contenttype, string accept)
        {

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Headers = headers;
            request.ContentType = contenttype;
            request.Method = method;
            request.Accept = accept;

            return request;
        }

        /// <summary>
        /// Build Web Response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetHttpWebResponse(HttpWebRequest request)
        {
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    return reader.ReadToEnd();
                }
            }
            catch (HttpException exception)
            {
                Console.WriteLine(exception);
                return string.Empty;
            
            }
        }

        /// <summary>
        /// PutJSON
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string PutJson(string url, string parameters)
        {
            string _json = "";
            byte[] bytes = Encoding.UTF8.GetBytes(parameters);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentLength = bytes.Length;
            request.ContentType = "application/x-www-form-urlencoded"; // "json";
            request.Method = "PUT";
            try
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        var reader = new StreamReader(response.GetResponseStream());
                        _json = reader.ReadToEnd();
                        response.Close();
                    }
                    else
                    {
                        //TraceUtility.TraceError("CommunityPutJSON - Unable to get JSON Stream");
                    }
                }
            }
            catch (WebException e)
            {
              //TODO:
            }
            return _json;
        }

        /// <summary>
        /// Post JSON
        /// </summary>
        /// <param name="url">URL endpoint</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>JSON string</returns>
        public static string PostJson(string url, string parameters)
        {
            string _json = "";
            byte[] bytes = Encoding.UTF8.GetBytes(parameters);
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.ContentLength = bytes.Length;
            request.ContentType = "application/x-www-form-urlencoded"; // "json";
            request.Method = "POST";
            request.Accept = "*/*";
           
            try
            {
                using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    _json = reader.ReadToEnd();
                    response.Close();
                }
                else
                {
                }
            }

        }

            catch (WebException e)
            {
               //TODO:
            }
            catch (Exception e)
            {
                string errormessage = e.Message;
            }
            return _json;
        }
     
        /// <summary>
        /// Post JSON
        /// </summary>
        /// <param name="url">URL endpoint</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>JSON string</returns>
        public static string PostJson(string url, string parameters, WebHeaderCollection headers)
        {
            string _json = "";
            byte[] bytes = Encoding.UTF8.GetBytes(parameters);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentLength = bytes.Length;           
            request.Method = "POST";
            request.Accept = "*/*";
            request.Headers = headers;
            request.ContentType = "application/json";

            try
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        var reader = new StreamReader(response.GetResponseStream());
                        _json = reader.ReadToEnd();
                        response.Close();
                    }
                    else
                    {
                        //TODO
                    }
                }

            }

            catch (WebException e)
            {
                //TODO
            }
            catch (Exception e)
            {
                string errormessage = e.Message;
            }
            return _json;
        }

        /// <summary>
        /// JSON Serialization 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON<T>(this T obj) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// JSON Serialization 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON_UTF8<T>(this T obj) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// JSON Deserialization
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="obj">object</param>
        /// <param name="json">JSON String</param>
        /// <returns></returns>
        public static T FromJSON<T>(this T obj, string json) where T : class
        {
            if (string.IsNullOrEmpty(json)) return null;

            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return serializer.ReadObject(stream) as T;
            }
        }

        /// <summary>
        /// Post JSON
        /// </summary>
        /// <param name="url">URL endpoint</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>JSON string</returns>
        public static string PostJson(string url, string parameters, string method, string contenttype)
        {
            string _json = "";
            byte[] bytes = Encoding.UTF8.GetBytes(parameters);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentLength = bytes.Length;
            request.ContentType = contenttype; 
            request.Method = method;
            request.Accept = "*/*";         
            request.ContentLength = bytes.Length;            
            try
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        var reader = new StreamReader(response.GetResponseStream());
                        _json = reader.ReadToEnd();
                        response.Close();
                    }
                    else
                    {                      
                    }
                }

            }

            catch (WebException e)
            {
               
            }
            catch (Exception e)
            {
                string errormessage = e.Message;
            }
            return _json;
        }

        /// <summary>
        /// Post JSON
        /// </summary>
        /// <param name="url">URL endpoint</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>JSON string</returns>
        public static string GetJsonWithTimeout(string url, string parameters, int timeout)
        {
            string _json = "";
            string full_url = url + "?" + parameters;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(full_url);
                request.Timeout = timeout; 

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        var reader = new StreamReader(response.GetResponseStream());
                        _json = reader.ReadToEnd();
                        response.Close();
                    }
                    else
                    {                       
                    }
                }

            }
            catch (WebException e)
            {
                throw (e);               
            }
            catch (Exception e2)
            {
              //TODO:
            }

            return _json;
        }

        /// <summary>
        /// Post JSON
        /// </summary>
        /// <param name="url">URL endpoint</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>JSON string</returns>
        public static string GetJson(string url, string parameters)
        {
            string _json = "";
            string full_url = url + "?" + parameters;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(full_url);
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        var reader = new StreamReader(response.GetResponseStream());
                        _json = reader.ReadToEnd();
                        response.Close();
                    }
                    else
                    {                       
                    }
                }

            }
            catch (WebException e)
            {             
            }
            catch (Exception e2)
            {
                //TODO:
            }

            return _json;
        }

        /// <summary>
        /// Post JSON
        /// </summary>
        /// <param name="url">URL endpoint</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>JSON string</returns>
        public static string GetJson(string url, string parameters, WebHeaderCollection headers, string contenttype, string accept)
        {
            string _json = "";
            string full_url = url + "?" + parameters;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(full_url);
                request.Headers = headers;
                request.ContentType = contenttype;
                request.Method = "GET";
                request.Accept = accept;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        var reader = new StreamReader(response.GetResponseStream());
                        _json = reader.ReadToEnd();
                        response.Close();
                    }
                    else
                    {                       
                    }
                }

            }
            catch (WebException e)
            {
                //TODO:
            }
            catch (Exception e2)
            {
                //TODO:
            }

            return _json;
        }

        public static string GetHostRequesterIpAddress()
        {
            string ipAddress = string.Empty;
            
            try
            {

                ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ipAddress != null && ipAddress.Length > 0)
                {
                    return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
                }
                else
                {
                    return HttpContext.Current.Request.UserHostAddress;
                }
             
            }
            catch (Exception e)
            {
                return string.Empty;            
            }
          
        }

        public static string GetUrlReferrerHost()
        {
            Uri urlReferrer;
            try
            {
                urlReferrer = HttpContext.Current.Request.UrlReferrer;
                return urlReferrer.Host;
            }
            catch { }
            {
                return string.Empty;
            }

        }

        public static void LogMessageToFile(string sMessage)
        {
            StreamWriter sw = File.AppendText(GetTempPath() + "acceptapi.log");

            try
            {
                string logLine = String.Format("{0:G}: {1}.", System.DateTime.Now, sMessage);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }           
        }

        public static string GetTempPath()
        {
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }
    }
}