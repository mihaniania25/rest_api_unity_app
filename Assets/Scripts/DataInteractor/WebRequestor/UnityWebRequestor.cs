using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace TABApps.TestTask
{
    public class UnityWebRequestor : IWebRequestor
    {
        private string _apiUrl;
        private string _resourceName;
        private string _baseRequestUrl;

        private Dictionary<CoroutineTask, WebRequestHandler> _tasksHandlers = new Dictionary<CoroutineTask, WebRequestHandler>();
        private Dictionary<WebRequestHandler, CoroutineTask> _handlersTasks = new Dictionary<WebRequestHandler, CoroutineTask>();
        private Dictionary<CoroutineTask, UnityWebRequest> _tasksUnityRequests = new Dictionary<CoroutineTask, UnityWebRequest>();

        public void Setup(ApiEndpointData endpointData)
        {
            ReadEndpointData(endpointData);
        }

        private void ReadEndpointData(ApiEndpointData endpointData)
        {
            _apiUrl = endpointData.ApiUrl;
            _resourceName = endpointData.ResourceName;

            if (_apiUrl.EndsWith('/') == false)
                _apiUrl += "/";

            _baseRequestUrl = _apiUrl + _resourceName;
        }

        public void Get(string id, WebRequestHandler requestHandler)
        {
            string requestUrl = GetRequestUrl(id);
            UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl);
            WebRequestResult requestResult = new WebRequestResult { ItemID = id };

            RegisterAndProcessRequest(webRequest, requestHandler, requestResult);
        }

        public void Get(WebRequestHandler requestHandler)
        {
            string requestUrl = GetRequestUrl();
            UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl);

            RegisterAndProcessRequest(webRequest, requestHandler, new WebRequestResult());
        }

        public void Put(string id, string bodyPart, WebRequestHandler requestHandler)
        {
            string requestUrl = GetRequestUrl(id);
            UnityWebRequest webRequest = UnityWebRequest.Put(requestUrl, bodyPart);
            webRequest.SetRequestHeader("Content-Type", "application/json"); 

            RegisterAndProcessRequest(webRequest, requestHandler, new WebRequestResult());
        }

        public void Post(WebRequestHandler requestHandler)
        {
            string requestUrl = GetRequestUrl();
            WWWForm wwwForm = new WWWForm();
            UnityWebRequest webRequest = UnityWebRequest.Post(requestUrl, wwwForm);

            RegisterAndProcessRequest(webRequest, requestHandler, new WebRequestResult());
        }

        public void Delete(string id, WebRequestHandler requestHandler)
        {
            string requestUrl = GetRequestUrl(id);
            UnityWebRequest webRequest = UnityWebRequest.Delete(requestUrl);
            WebRequestResult requestResult = new WebRequestResult { ItemID = id };

            RegisterAndProcessRequest(webRequest, requestHandler, requestResult);
        }

        private string GetRequestUrl(string id = null)
        {
            if (string.IsNullOrEmpty(id))
                return _baseRequestUrl;
            return $"{_baseRequestUrl}/{id}";
        }

        private void RegisterAndProcessRequest(UnityWebRequest webRequest, WebRequestHandler requestHandler, WebRequestResult requestResult)
        {
            CoroutineTask task = new CoroutineTask(ProcessRequest(webRequest, requestHandler, requestResult));

            _handlersTasks[requestHandler] = task;
            _tasksHandlers[task] = requestHandler;
            _tasksUnityRequests[task] = webRequest;
        }

        private IEnumerator ProcessRequest(UnityWebRequest webRequest, WebRequestHandler requestHandler, WebRequestResult requestResult)
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    requestResult.Succeed = true;
                    requestResult.Message = webRequest.downloadHandler?.text ?? "";
                    break;
                default:
                    requestResult.Succeed = false;
                    requestResult.Message = webRequest.error;
                    requestResult.ErrorCode = webRequest.responseCode;
                    break;
            }

            HandleRequestResult(requestHandler, requestResult);
        }

        private void HandleRequestResult(WebRequestHandler requestHandler, WebRequestResult requestResult)
        {
            CoroutineTask requestTask = _handlersTasks[requestHandler];
            UnityWebRequest unityWebRequest = _tasksUnityRequests[requestTask];

            _handlersTasks.Remove(requestHandler);
            _tasksHandlers.Remove(requestTask);

            unityWebRequest.Dispose();
            _tasksUnityRequests.Remove(requestTask);

            requestHandler?.Invoke(requestResult);
        }

        public void Dispose()
        {
            List<CoroutineTask> tasksToRemove = _tasksHandlers.Keys.ToList();

            foreach (CoroutineTask task in tasksToRemove)
            {
                task?.Stop();

                UnityWebRequest unityWebRequest = _tasksUnityRequests[task];
                unityWebRequest.Dispose();
            }

            _tasksHandlers.Clear();
            _handlersTasks.Clear();
            _tasksUnityRequests.Clear();
        }
    }
}