using System.Net.Http;

namespace MangaDownloader.CloudFlare
{
    internal static class HttpMessageHandlerExtensions
    {
        public static HttpMessageHandler GetMostInnerHandler(this HttpMessageHandler self)
        {
            return self is DelegatingHandler handler
                ? handler.InnerHandler.GetMostInnerHandler()
                : self;
        }
    }
}