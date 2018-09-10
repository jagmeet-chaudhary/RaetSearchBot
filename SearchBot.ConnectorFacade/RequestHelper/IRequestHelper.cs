using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors
{
    public interface IRequestHelper
    {
        void Init(string baseUriConfigKey);
        void AddQueryStringParameter(string key, object value);
        void RemoveQueryStringParameter(string key);
        void AddClientHeader(string key, object value);
        void UpdateClientHeader(string key, object value);
        void RemoveClientHeader(string key);

        TimeSpan Timeout { get; set; }

        string BaseUri { get; set; }

        string AuthenticationToken { get; set; }

        Task<T> GetAsync<T>(string path);


        Task<HttpResponseMessage> GetWithResponseAsync(string path);
        
    }
}
