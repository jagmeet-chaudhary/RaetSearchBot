using SearchBot.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;


namespace SearchBot.Connectors
{
    public static class HttpClientExtensions
    {
        public static void AddHeadersToRequest(this HttpClient client, IDictionary<string, object> clientHeaders)
        {
            HandleLanguageSettings(clientHeaders);

            foreach (KeyValuePair<string, object> header in clientHeaders)
            {
                try
                {
                    client.AddOrUpdateHeader(header);
                }
                catch (Exception ex)
                {
                    LogFactory.Log.Error($"Fatal error adding headers to request : {ex}");
                    throw;
                }
            }
        }

        private static void AddOrUpdateHeader(this HttpClient client, KeyValuePair<string, object> header)
        {
            if (header.Value is string)
            {
                AddOrUpdateHeader(client, header.Key, (string)header.Value);
                return;
            }

            if (header.Value is IEnumerable<string>)
            {
                AddOrUpdateHeader(client, header.Key, (IEnumerable<string>)header.Value);
                return;
            }

            client.DefaultRequestHeaders.Add(header.Key, header.Value.SerializeObjectToBase64());
        }

        private static void AddOrUpdateHeader(this HttpClient client, string headerKey, string headerValue)
        {
            if (headerKey == null)
            {
                LogFactory.Log.Error($"Null headerKey value: client: '{client}'  and new header value {headerValue}");
                return;
            }
            if (headerValue == null)
            {
                LogFactory.Log.Error($"Null headerValue value: client: '{client}'  and new headerKey {headerKey}");
                return;
            }

            if (client.DefaultRequestHeaders.Contains(headerKey))
            {
                client.DefaultRequestHeaders.Remove(headerKey);
            }

            client.DefaultRequestHeaders.Add(headerKey, headerValue);
        }

        private static void AddOrUpdateHeader(this HttpClient client, string headerKey, IEnumerable<string> headerValue)
        {
            if (headerKey == null)
            {
                LogFactory.Log.Error($"Null headerKey value: client: '{client}'  and new header value {headerValue}");
                return;
            }
            if (headerValue == null)
            {
                LogFactory.Log.Error($"Null headerValue value: client: '{client}'  and new headerKey {headerKey}");
                return;
            }

            if (client.DefaultRequestHeaders.Contains(headerKey))
            {
                client.DefaultRequestHeaders.Remove(headerKey);
            }

            client.DefaultRequestHeaders.Add(headerKey, headerValue);
        }

        private static void HandleLanguageSettings(IDictionary<string, object> clientHeaders)
        {
            const string accept = "Accept-Language";
            var currentContext = HttpContext.Current;

            if (HasUserLanguages(currentContext))
            {
                clientHeaders[accept] = currentContext?.Request.UserLanguages;
            }
            else if (currentContext == null && clientHeaders.ContainsKey(accept))
            {
                //use the language sent in the header
            }
            else
            {
                clientHeaders[accept] = "en-US";
            }
        }

        private static bool HasUserLanguages(HttpContext currentContext)
        {
            return (currentContext?.Handler != null && currentContext.Request.UserLanguages != null);
        }
    }
}