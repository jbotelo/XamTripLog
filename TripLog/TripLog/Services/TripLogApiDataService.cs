using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TripLog.Models;

namespace TripLog.Services
{
    public class TripLogApiDataService : BaseHttpService, ITripLogDataService
    {
        private readonly Uri _baseUri;
        private readonly IDictionary<string, string> _headers;
        public Action<string> AuthorizedDelegate { get; set; }
        public Action UnauthorizedDelegate { get; set; }

        public TripLogApiDataService(Uri baseUri, string authToken)
        {
            _baseUri = baseUri;
            _headers = new Dictionary<string, string>();
            _headers.Add("x-zumo-auth", authToken);

            // TODO: Add header with auth-based token in chapter 7
        }


        public async Task<TripLogEntry> AddEntryAsync(TripLogEntry entry)
        {
            try
            {
                var url = new Uri(_baseUri, "/api/entries");
                var response = await SendRequestAsync<TripLogEntry>(url,
                    HttpMethod.Post, _headers, entry);

                return response;
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedDelegate?.Invoke();
                throw;
            }
        }

        public async Task<IList<TripLogEntry>> GetEntriesAsync()
        {
            try
            {
                var url = new Uri(_baseUri, "/api/entries");
                var response = await SendRequestAsync<TripLogEntry[]>(url,
                    HttpMethod.Get, _headers);

                return response;
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedDelegate?.Invoke();
                throw;
            }
        }

        public async Task AuthenticateAsync(string idProvider, string idProviderToken)
        {
            var token = new IdProviderToken
            {
                AccessToken = idProviderToken
            };

            var url = new Uri(_baseUri,
                string.Format(".auth/login/{0}", idProvider));
            var response = await SendRequestAsync<TripLogApiAuthToken>(url,
                HttpMethod.Post, requestData: token);

            if (!string.IsNullOrWhiteSpace(response?.AuthenticationToken))
            {
                var authToken = response.AuthenticationToken;
                // Update this service with the new auth token
                _headers["x-zumo-auth"] = authToken;
                AuthorizedDelegate?.Invoke(authToken);
            }
        }

        public void Unauthenticate() => UnauthorizedDelegate?.Invoke();
        struct IdProviderToken
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }
    }
}
