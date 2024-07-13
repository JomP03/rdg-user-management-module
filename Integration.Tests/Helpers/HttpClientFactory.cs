namespace Integration.Tests.Helpers
{
    internal class HttpClientFactory : IHttpClientFactory
    {
        internal HttpClientFactory() { }

        public HttpClient CreateClient(string name) => new();
    }
}
