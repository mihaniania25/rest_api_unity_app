namespace TABApps.TestTask
{
    public class WebRequestorGetter
    {
        public IWebRequestor GetWebRequestor()
        {
            return new UnityWebRequestor();
        }
    }
}