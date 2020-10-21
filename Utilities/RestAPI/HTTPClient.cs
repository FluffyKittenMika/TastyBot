namespace Utilities.RestAPI
{
    public static class HTTPClient
    {
        private static readonly System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient();
        
        public static System.Net.Http.HttpClient GetHttpClient()
        {
            return _httpClient;
        }
    }
}
