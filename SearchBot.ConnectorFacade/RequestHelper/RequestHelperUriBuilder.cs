using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SearchBot.Connectors
{
    public class RequestHelperUrlBuilder
    {
        public static string BuildUrl(string baseUri, string path, IDictionary<string, string> queryStringParameters)
        {
            var queryString = new StringBuilder();
            if (queryStringParameters.Count <= 0)
                return $"{baseUri}{(string.IsNullOrEmpty(path) ? "" : $"/{path}")}";

            var first = true;
            queryString.Append("?");
            foreach (var item in queryStringParameters)
            {
                if (!first)
                    queryString.Append("&");
                first = false;
                queryString.Append($"{item.Key}={HttpUtility.UrlEncode(item.Value)}");
            }

            return $"{baseUri}{(string.IsNullOrEmpty(path) ? "" : $"/{path}")}{queryString}";
        }
    }
}
