using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptApi.Areas.Api.Models;
using AcceptApi.Areas.Api.Models.Core;
using AcceptApi.Areas.Api.Models.PostEdit;
using AcceptApi.Areas.Api.Models.Utils;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AcceptApi.Areas.RealTimeApi.Hubs
{
    [HubName("postEdit")]
    public class PostEditHub:Hub
    {      

        public void ParaphrasingSync()
        {
            Clients.Client(Context.ConnectionId).handleSync("synced for connection: " + Context.ConnectionId); 
        }

        public void Paraphrasing(string maxResults, string sysId, string lang, string source, string target, string context)
        {
            string query; query = string.Empty;
            try
            {              
                //content to check.
                query = HttpUtility.UrlDecode(target, System.Text.Encoding.UTF8);
                query = query.Replace("?", "%3F");
                //the paraprasing response object.
                ParaphraseResponse response; response = new ParaphraseResponse();
                //parse the paraphrasing api result.
                JObject jObject; jObject = new JObject();
                string jsonRawResponse;
                try
                {
                    jsonRawResponse = AcceptApiWebUtils.GetJsonWithTimeout(AcceptApiCoreUtils.ParaphrasingEndpoint, "max=" + maxResults + "&sys=" + sysId + "&lang=" + lang + "&q=" + query + "", AcceptApiCoreUtils.ParaphrasingTimeoutPeriod);
                }
                catch (Exception e)
                {
                    throw (e);
                }


                try
                {
                    jObject = JObject.Parse(jsonRawResponse);
                    //query the json object: looking for the paraphrases property.                         
                    response.resultSet = JsonConvert.DeserializeObject<List<string[]>>(jObject["paraphrases"].ToString());
                    response.status = "OK";
                    response.error = string.Empty;
                }
                catch
                {            
                    try
                    {
                        response.resultSet = new List<string[]>();
                        response.error = JsonConvert.DeserializeObject<string>(jObject["error"].ToString());
                        response.status = JsonConvert.DeserializeObject<string>(jObject["status"].ToString());
                    }
                    catch
                    {
                        throw (new Exception("error parsing raw paraphrase api response status."));
                    }


                }

                Clients.Client(Context.ConnectionId).handleParaphrasingResponse(Context.ConnectionId, "OK", response, null);                
            }
            catch (Exception e)
            {
                Clients.Client(Context.ConnectionId).handleParaphrasingResponse(Context.ConnectionId, "FAILED:"+ e.Message, new ParaphraseResponse(), "remote request payload: " + AcceptApiCoreUtils.ParaphrasingEndpoint + "?" + "max=" + maxResults + "&sys=" + sysId + "&lang=" + lang + "&q=" + query );
            }
        }     
    }
}