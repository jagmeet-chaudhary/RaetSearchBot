using Newtonsoft.Json;
using SearchBot.Common;
using SearchBot.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchBot.Connectors
{
    public class RequestHelper : IRequestHelper 
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly Dictionary<string, object> _clientHeaders = new Dictionary<string, object>();
        private Dictionary<string, string> _queryStringParameters;
        protected IDictionary<string, string> QueryStringParameters => _queryStringParameters ?? (_queryStringParameters = new Dictionary<string, string>());

        public virtual HttpClient Client
        {
            get
            {
                return httpClient;
            }
        }

        public TimeSpan Timeout
        {
            get
            {
                return httpClient.Timeout;
            }
            set
            {
                httpClient.Timeout = value;
            }
        }

        public string BaseUri { get; set; }

        public string AuthenticationToken { get; set; }

        public void Init(string baseUriConfigKey)
        {
            BaseUri = ConfigurationManager.AppSettings[baseUriConfigKey];
            
            if (string.IsNullOrWhiteSpace(this.BaseUri))
            {
                throw new ApplicationException($"{baseUriConfigKey} not configured.");
            }
        }

        public void AddQueryStringParameter(string key, object value)
        {
            

            string stringValue;
            if (value is string)
            {
                stringValue = value.ToString();
            }
            else
            {
                stringValue = value.SerializeObjectToBase64();
            }

            QueryStringParameters.Add(new KeyValuePair<string, string>(key, stringValue));
        }

        public void RemoveQueryStringParameter(string key)
        {
            if (!QueryStringParameters.ContainsKey(key))
                return;

            QueryStringParameters.Remove(key);
        }

        public void AddClientHeader(string key, object value)
        {
            if (_clientHeaders.ContainsKey(key))
            {
                _clientHeaders[key] = value;
            }
            else
            {
                _clientHeaders.Add(key, value);
            }
        }

        public void UpdateClientHeader(string key, object value)
        {
            if (!_clientHeaders.ContainsKey(key))
            {
                throw new InvalidOperationException("RequestHelper.UpdateClientHeader. Error: trying to update a nonexistent header.");
            }
            _clientHeaders[key] = value;
        }

        public void RemoveClientHeader(string key)
        {
            if (!_clientHeaders.ContainsKey(key))
            {
                throw new InvalidOperationException("RequestHelper.RemoveClientHeader. Error: trying to remove a nonexistent header.");
            }
            _clientHeaders.Remove(key);
        }

        public async Task<T> GetAsync<T>(string path)
        {
            var response = await GetWithResponseAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                LogFactory.Log.Error($"RequestHelper.GetAsync returned non successful status code: {path}");
                if (response.StatusCode == HttpStatusCode.Forbidden)
                    throw new NotAuthorizedException();

                throw new Exception($"Incorrect response with status code : {response.StatusCode}");
            }

            T deserialized = default(T);

            try
            {
                deserialized = await response.Content.ReadAsAsync<T>();
            }
            catch (JsonReaderException ex)
            {
                var body = response.Content.ReadAsStringAsync();
                var text = $"Could not convert {body.Result} into type: {typeof(T).Name} calling path: {path}";

                throw new ArgumentException(text, ex);
            }
            return deserialized;
        }


        public async Task<HttpResponseMessage> GetWithResponseAsync(string path)
        {
            HttpResponseMessage response = null;

            //LogFactory.Log.Debug($"Before RequestHelper GetWithResponseAsync request: {path}");

            try
            {
                string urlPath = PrepareRequest(path);
                var syncContext = SynchronizationContext.Current;
                SynchronizationContext.SetSynchronizationContext(null);
                //return await ExecuteGetAsync(urlPath);
                var data = await ExecuteGetAsync(urlPath);

                SynchronizationContext.SetSynchronizationContext(syncContext);
                return data;
            }
            catch (WebException ex)
            {
               
                LogFactory.Log.Error(ex.Message);
                
                throw ex;
            }
            catch (Exception ex)
            {
                LogFactory.Log.Error(ex.Message);
                throw ex;
            }
        }

        protected virtual async Task<HttpResponseMessage> ExecuteGetAsync(string urlPath)
        {
           LogFactory.Log.Info($"Before RequestHelper ExecuteGetAsync request: {urlPath}");

            return await httpClient.GetAsync(urlPath);
        }

        private string PrepareRequest(string path)
        {
            // TODO: This sequence is very obscure:
            AddCommonHeaders(httpClient); // This adds authentication header to the HttpClient and CorrelationId to the _clientHeaders, not the HttpClient.
            var temporalDictionary = new Dictionary<string, object>(_clientHeaders); // This creates a COPY of the _clientHeaders.
            httpClient.AddHeadersToRequest(temporalDictionary); // This uses the COPY to create request headers and add them to the HttpClient.

            var urlPath = RequestHelperUrlBuilder.BuildUrl(BaseUri, path, QueryStringParameters);

            LogFactory.Log.Info($"RequestHelper PrepareRequest: {urlPath}");

            return urlPath;
        }

        private void AddCommonHeaders(HttpClient client)
        {
            if (AuthenticationToken != null)
            {
                if (client.DefaultRequestHeaders.Authorization != null)
                    client.DefaultRequestHeaders.Remove(nameof(client.DefaultRequestHeaders.Authorization));

                if (!string.IsNullOrEmpty(this.AuthenticationToken))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthenticationToken);
            }

            
        }
    }
}
