using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AcceptApi.Areas.Api.Models;
using AcceptApi.Areas.Api.Models.Core;
using AcceptApi.Areas.Api.Models.Utils;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AcceptApi.Areas.RealTimeApi.Hubs
{
    [HubName("preEdit")]
    public class PreEditHub:Hub
    {              
        private int sleepPeriod = 2000;      
        public PreEditHub()
        {
          
        }

        #region check content.
        
        #region go serial

        public void AcceptRequest(string[] textSegments, string language, string rule, string grammar, string spell,
          string style, string requestFormat, string apiKey, string globalSessionId, string sessionMetaData, string origin, string clientIndex)
        {

            try
            {
                Dictionary<string, string> parameters; parameters = new Dictionary<string, string>();                
                parameters.Add("text", string.Empty);
                parameters.Add("languageid", language);
                parameters.Add("ruleset", rule);
                parameters.Add("grammar", grammar);
                parameters.Add("spell", spell);
                parameters.Add("style", style);
                parameters.Add("username", AcceptApiCoreUtils.AcrolinxUser);
                parameters.Add("password", AcceptApiCoreUtils.AcrolinxUserPassword);
                parameters.Add("requestFormat", requestFormat);
                if (sessionMetaData != null && sessionMetaData.Length > 0)
                    parameters.Add("sessionMetadata", sessionMetaData);

                if (apiKey != null && apiKey.Length > 0)
                    parameters.Add("apiKey", apiKey);    
                
                PreEditWorker(parameters, textSegments, origin, string.Empty, int.Parse(clientIndex));
            }
            catch (Exception e)
            {
                Clients.Client(Context.ConnectionId).handleAcceptResponseException(Context.ConnectionId, e.Message);
            }
        }

        private async void PreEditWorker(Dictionary<string, string> parameters, string[] textSegments, string requestOrigin, string initialSessionCodeId, int clientIndex)
        {
            for (int i = 0; i < textSegments.Length; i++)
            {
                try
                {
                    //Clients.Client(Context.ConnectionId).logAccept("starting a new thread for segment: " + i.ToString() + " at: " + DateTime.UtcNow.ToString());
                    Task<CoreApiResponse> segmentResponseThread = DoSegmentWork(parameters, requestOrigin, i, textSegments[i]);
                    CoreApiResponse segmentResponse = await segmentResponseThread;
                    if (Context != null && Context.ConnectionId != null && segmentResponse != null && segmentResponse.ResponseStatus == CoreApiResponseStatus.Ok.ToString())
                        Clients.Client(Context.ConnectionId).handleAcceptResponse(Context.ConnectionId, segmentResponse, textSegments.Length > 1 ? i : clientIndex);
                    else
                        Clients.Client(Context.ConnectionId).handleAcceptResponseException(Context.ConnectionId, segmentResponse);
                }
                catch (Exception e) { throw new Exception("Exception Thrown During PreEditWorker Loop: " + e.Message); }
            }
        }

        private async Task<CoreApiResponse> DoSegmentWork(Dictionary<string, string> parameters, string requestOrigin, int index, string currentSegmentText)
        {
            CoreApiResponse model; model = null;
            IAcceptApiManager acceptCoreManager; acceptCoreManager = new AcceptApiManager();

            try
            {
                parameters["text"] = HttpUtility.UrlDecode(currentSegmentText, System.Text.Encoding.UTF8);                
                model = acceptCoreManager.GenericRealTimeRequest(parameters, Context.ConnectionId, requestOrigin, index.ToString());
            }
            catch (Exception e)
            {
                Clients.Client(Context.ConnectionId).logAccept("exception on call GenericRealTimeRequest:" + e.Message);
            }

            if (model != null && model.ResponseStatus == CoreApiResponseStatus.Ok)
            {
                try
                {
                    var delayPeriod = sleepPeriod;
                    int i = 0;
                    while (i < 10)
                    {
                        //Clients.Client(Context.ConnectionId).logAccept("about to get response status on segment:" + index.ToString());
                        var segmentStatus = (AcceptApiResponseStatus)GetResponseStatus(model.AcceptSessionCode);

                        if (segmentStatus != null && segmentStatus.ResponseStatus == CoreApiResponseStatus.Ok)
                        {
                            if (segmentStatus.State == "DONE")
                            {
                                //Clients.Client(Context.ConnectionId).logAccept("getting the response on segment:" + index.ToString());
                                CoreApiResponse response = GetResponse(model.AcceptSessionCode, model.GlobalSessionId);
                                if(response != null && response.GlobalSessionId != null && response.GlobalSessionId.Length > 0)
                                    CompleteSession(response.GlobalSessionId, parameters["text"], acceptCoreManager);
                                return response;
                            }
                            else
                                if (segmentStatus.State == "FAILED")
                                    return new CoreApiException("State == Failed.", "DoParallelSegmentWork");

                        }
                        else
                            if (segmentStatus.ResponseStatus == CoreApiResponseStatus.Failed)
                            {
                                return new CoreApiException("ResponseStatus == Failed.", "DoParallelSegmentWork");
                            }

                        //Clients.Client(Context.ConnectionId).logAccept("await on segment: " + index.ToString());
                        await Task.Delay(delayPeriod);
                        //Clients.Client(Context.ConnectionId).logAccept("finish await on segment: " + index.ToString());
                        i++;
                    }
                    //Clients.Client(Context.ConnectionId).logAccept("while loop ended for segment: " + index.ToString());
                }
                catch (Exception e) { throw new Exception("Exception Thrown on DoSegmentWork Loop: " + e.Message); }
            }

            return new CoreApiException("No Results After Loop For Segment: " + index.ToString(), "DoSegmentWork");
        }
       
        #endregion

        #region go parallel
        public void AcceptParallelRequest(string[] textSegments, string language, string rule, string grammar, string spell,
           string style, string requestFormat, string apiKey, string globalSessionId, string sessionMetaData, string origin)
        {                     
            try
            {
                Dictionary<string, string> parameters; parameters = new Dictionary<string, string>();               
                parameters.Add("text", string.Empty);
                parameters.Add("languageid", language);
                parameters.Add("ruleset", rule);
                parameters.Add("grammar", grammar);
                parameters.Add("spell", spell);
                parameters.Add("style", style);
                parameters.Add("username", AcceptApiCoreUtils.AcrolinxUser);
                parameters.Add("password", AcceptApiCoreUtils.AcrolinxUserPassword);
                parameters.Add("requestFormat", requestFormat);               
                if (sessionMetaData != null && sessionMetaData.Length > 0)
                    parameters.Add("sessionMetadata", sessionMetaData);

                if (apiKey != null && apiKey.Length > 0)
                    parameters.Add("apiKey", apiKey);    

                PreEditParallelWorker(parameters, textSegments, origin, string.Empty);              
            }
            catch (Exception e)
            {
                Clients.Client(Context.ConnectionId).handleAcceptResponseException(Context.ConnectionId, e.Message);
            }
        }
   

        private async void PreEditParallelWorker(Dictionary<string, string> parameters, string[] textSegments, string requestOrigin, string initialSessionCodeId)
        {
            var tasks = new List<Task<CoreApiResponse>>();           
            for (int i = 0; i < textSegments.Length; i++)
            {
                try
                {
                    tasks.Add(DoParallelSegmentWork(parameters, requestOrigin, i, textSegments[i]));
                }
                catch (Exception e) { throw new Exception("Exception Thrown During PreEditParallelWorker(populating thread pool) Loop: " + e.Message); }
            }
          
            foreach (var task in await Task.WhenAll(tasks))
            {
                Clients.Client(Context.ConnectionId).handleAcceptParallelResponseCompleted(Context.ConnectionId, "tasks finished.");               
            }
        }

        private async Task<CoreApiResponse> DoParallelSegmentWork(Dictionary<string, string> parameters, string requestOrigin, int index, string currentSegmentText)
        {
            CoreApiResponse model; model = null;
            IAcceptApiManager acceptCoreManager; acceptCoreManager = new AcceptApiManager();

            try
            {
                parameters["text"] = HttpUtility.UrlDecode(currentSegmentText, System.Text.Encoding.UTF8);                
                model = acceptCoreManager.GenericRealTimeRequest(parameters, Context.ConnectionId, requestOrigin, index.ToString());
            }
            catch (Exception e) {
                Clients.Client(Context.ConnectionId).logAccept("exception on call GenericRealTimeRequest:" + e.Message);
            }

            if (model != null && model.ResponseStatus == CoreApiResponseStatus.Ok)
            {
                try
                {
                    var delayPeriod = sleepPeriod;
                    int i = 0;
                    while (i < 10)
                    {
                        var segmentStatus = new AcceptApiResponseStatus();

                        try
                        {
                            //Clients.Client(Context.ConnectionId).logAccept("about to get response status on segment:" + index.ToString());
                            segmentStatus = (AcceptApiResponseStatus)GetResponseStatus(model.AcceptSessionCode);
                        }
                        catch (Exception e)
                        {
                            Clients.Client(Context.ConnectionId).logAccept("exception on to get response status on segment:" + e.Message);
                        }

                        if (segmentStatus != null && segmentStatus.ResponseStatus == CoreApiResponseStatus.Ok)
                        {
                            if (segmentStatus.State == "DONE")
                            {
                                //Clients.Client(Context.ConnectionId).logAccept("getting the response on segment:" + index.ToString());
                                try
                                {
                                    CoreApiResponse response = GetResponse(model.AcceptSessionCode, model.GlobalSessionId);
                                    //Clients.Client(Context.ConnectionId).logAccept("about to log response on segment: " + index.ToString());
                                    Clients.Client(Context.ConnectionId).handleAcceptParallelResponse(Context.ConnectionId, response, index);
                                    if (response != null && response.GlobalSessionId != null && response.GlobalSessionId.Length > 0)
                                        CompleteSession(response.GlobalSessionId, parameters["text"], acceptCoreManager);
                                    return new CoreApiResponse();

                                }catch(Exception e)
                                {
                                    Clients.Client(Context.ConnectionId).logAccept("exception on to get response on segment:" + e.Message);
                                }
                            }
                            else
                                if (segmentStatus.State == "FAILED")
                                    return new CoreApiException("State == Failed.", "DoParallelSegmentWork");

                        }
                        else
                            if (segmentStatus.ResponseStatus == CoreApiResponseStatus.Failed)
                            {
                                return new CoreApiException("ResponseStatus == Failed.", "DoParallelSegmentWork");
                            }

                        //Clients.Client(Context.ConnectionId).logAccept("await on segment: " + index.ToString());
                        await Task.Delay(delayPeriod);
                        //Clients.Client(Context.ConnectionId).logAccept("finish await on segment: " + index.ToString());
                        i++;
                    }
                    //Clients.Client(Context.ConnectionId).logAccept("while loop ended for segment: " + index.ToString());
                }
                catch (Exception e) { throw new Exception("Exception Thrown on DoParallelSegmentWork Loop: " + e.Message); }
            }

            return new CoreApiException("No Results After Loop For Segment: " + index.ToString(), "DoParallelSegmentWork");
        }
        
        #endregion

        #region shared methods
        private CoreApiResponse GetResponse(string session, string globalSessionId)
        {

            try
            {
                IAcceptApiManager acceptCoreManager = new AcceptApiManager();
                Dictionary<string, string> parameters; parameters = new Dictionary<string, string>();
                parameters.Add("session", session);

                try
                {
                    var model = acceptCoreManager.GetRealTimeResponse(parameters);
                    model.GlobalSessionId = globalSessionId;
                    return model;
                }
                catch
                {
                    return new AcceptApiResponseStatus(CoreApiResponseStatus.Failed, session);
                }
            }
            catch (Exception e)
            {
                Clients.Client(Context.ConnectionId).logAccept("exception during get response:" + e.Message);
                return new AcceptApiResponseStatus(CoreApiResponseStatus.Failed, session);                
            }
        }

        private CoreApiResponse GetResponseStatus(string session)
        {
            try
            {
                IAcceptApiManager acceptCoreManager = new AcceptApiManager();
                Dictionary<string, string> parameters; parameters = new Dictionary<string, string>();
                parameters.Add("session", session);

                try
                {
                    var model = acceptCoreManager.GetRealTimeResponseStatus(parameters);
                    return model;
                }
                catch
                {
                    return new AcceptApiResponseStatus(CoreApiResponseStatus.Failed, session);
                }

            }
            catch (Exception e)
            {
                Clients.Client(Context.ConnectionId).logAccept("exception during get response status:" + e.Message);
                return new AcceptApiResponseStatus(CoreApiResponseStatus.Failed, session);                
            }
        }

        private void CompleteSession(string globalSessionId, string textContent, IAcceptApiManager acceptCoreManager)
        {
            var model = acceptCoreManager.AuditFinalContext(globalSessionId, textContent, DateTime.UtcNow);                    
        }
        #endregion

        #endregion

        #region test methods.
        public void AcceptRequestEcho(string[] textSegments, string language, string rule, string grammar, string spell,
              string style, string requestFormat, string apiKey, string globalSessionId, string sessionMetaData)
        {
            
            try
            {
                var model = new { text = textSegments[0] };
                Clients.Client(Context.ConnectionId).handleEchoResponse(Context.ConnectionId, model, 0);            
            }
            catch (Exception e)
            {
                Clients.Client(Context.ConnectionId).handleAcceptResponseException(Context.ConnectionId, e.Message);            
            }
        }

        public void TestAcceptRequest(int x, int y)
        {
            Clients.Others.shapeMoved(Context.ConnectionId, x, y);           
        }
        #endregion

        #region audit methods.

        public void AuditFlag(string globalSessionId, string flag, string userAction, string actionValue, string ignored, string name,
            string textBefore, string textAfter, string timeStamp, string jsonRaw, string privateId)
        {                         
            try
            {
                IAcceptApiManager acceptCoreManager = new AcceptApiManager();
                var model = acceptCoreManager.AddAuditFlag(HttpUtility.UrlDecode(flag, System.Text.Encoding.UTF8),
                    HttpUtility.UrlDecode(userAction, System.Text.Encoding.UTF8),
                    HttpUtility.UrlDecode(actionValue, System.Text.Encoding.UTF8),
                    globalSessionId, HttpUtility.UrlDecode(ignored, System.Text.Encoding.UTF8), name, 
                    HttpUtility.UrlDecode(textBefore, System.Text.Encoding.UTF8), 
                    HttpUtility.UrlDecode(textAfter, System.Text.Encoding.UTF8), 
                    DateTime.UtcNow, HttpUtility.UrlDecode(jsonRaw, System.Text.Encoding.UTF8),
                    privateId);           
            }
            catch (Exception e)
            {
                Clients.Client(Context.ConnectionId).handleAcceptResponseException(Context.ConnectionId, e.Message);                      
            }             
        }
        

        public void AuditFinalContext(string globalSessionId, string textContent, string timeStamp)
        {                 
            try
            {
                IAcceptApiManager acceptCoreManager = new AcceptApiManager();
                acceptCoreManager.AuditFinalContext(globalSessionId, HttpUtility.UrlDecode(textContent, System.Text.Encoding.UTF8), DateTime.UtcNow);
               
            }
            catch (Exception e)
            {
                Clients.Client(Context.ConnectionId).handleAcceptResponseException(Context.ConnectionId, e.Message);
            }                           
        }
        
        #endregion
    }
}