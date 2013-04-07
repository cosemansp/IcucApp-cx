using System;
using System.Collections.Generic;
using System.Net;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Net;
using RestSharp;

namespace IcucApp.Core.Services.PushWoosh
{
    public class PushWooshAgent : IPushWooshAgent
    {
        private const string Host = "https://cp.pushwoosh.com/";
        private const string RegisterRequest = "registerDevice";
        //private const string UnregisterRequest = "unregisterDevice";
        private const string TagsRequest = "setTags";
        //private const string GeozoneRequest = "getNearestZone";
        private const string RequestDomain = Host + "json/1.3/";

        private static readonly ILog Log = LogManager.GetLogger(typeof (PushWooshAgent));

        public void RegisterDevice(string appCode, string deviceToken, string hardwareId)
        {
            Log.InfoFormat("RegisterDevice - App: {0}, DeviceToken: {1}, hwID: {2}", appCode, deviceToken, hardwareId);

            // Setup client
            var client = new RestClient(RequestDomain);

            // Build request
            var message = new RegistrationMessage(appCode, deviceToken, hardwareId);
            var request = new RestRequest(RegisterRequest, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(message);

            // Execute
            var response = client.Execute<StatusMessage>(request);
            HandleResponse(response);

            Log.InfoFormat("  Result: {0}", response.Content);
        }

        public void UnregisterDevice(string appCode, string hardwareId)
        {
            throw new NotImplementedException();
        }

        public void SetTags(string appCode, string hardwareId, Dictionary<string, object> tags)
        {
            Log.InfoFormat("SetTags - App: {0}, hwID: {1}", appCode, hardwareId);

            // Setup client
            var client = new RestClient(RequestDomain);

            // Build request
            var message = new SetTagsMessage(appCode, hardwareId, tags);
            var request = new RestRequest(TagsRequest, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(message);

            // Execute
            var response = client.Execute<SetTagsResultMessage>(request);
            var data = HandleResponse(response);
            Log.InfoFormat("  Result: {0}", response.Content);
            if (data.response != null && data.response.skipped.Count > 0)
            {
                throw new PushWooshSetTagException("Failed to set tags", data.response.skipped);
            }
        }

        public void GetNearestZone(string appCode, string hardwareId, string lat, string lng)
        {
            throw new NotImplementedException();
        }

        public T HandleResponse<T>(IRestResponse<T> response) where T : StatusMessage
        {
            // handle http errors
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new CommunicationException("Failed to SetTags: HTTP ERROR: " + response.StatusCode);
            }

            // handle invalid response data
            if (response.Data == null)
            {
                throw new CommunicationException("Failed to SetTags: Invalid response: " + response.Content);
            }

            // handle PushWoosh errors
            Log.InfoFormat("  Result Service: {0} - {1}", response.Data.status_code, response.Data.status_message);
            if (response.Data.status_code != "200")
            {
                throw new PushWooshException("Failed to register device", response.Data.status_code,
                                                response.Data.status_message);
            }
            return response.Data;
        }
    }
}