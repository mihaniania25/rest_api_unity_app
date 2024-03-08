namespace TABApps.TestTask
{
    public interface IWebRequestor
    {
        void Setup(ApiEndpointData endpointData);
        void Get(string id, WebRequestHandler requestHandler);
        void Get(WebRequestHandler requestHandler);
        void Put(string id, string bodyPart, WebRequestHandler requestHandler);
        void Post(WebRequestHandler requestHandler);
        void Delete(string id, WebRequestHandler requestHandler);
        void Dispose();
    }
}