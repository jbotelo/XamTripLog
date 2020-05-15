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

        public TripLogApiDataService(Uri baseUri)
        {
            _baseUri = baseUri;
            _headers = new Dictionary<string, string>();

            // TODO: Add header with auth-based token in chapter 7
        }
        
        public async Task<TripLogEntry> AddEntryAsync(TripLogEntry entry)
        {
            var url = new Uri(_baseUri, "/api/entries");
            var response = await SendRequestAsync<TripLogEntry>(url,
                HttpMethod.Post, _headers, entry);
            
            return response;
        }

        public async Task<IList<TripLogEntry>> GetEntriesAsync()
        {
            var url = new Uri(_baseUri, "/api/entries");
            var response = await SendRequestAsync<TripLogEntry[]>(url,
                HttpMethod.Get, _headers);
            
            return response;
        }
    }
}
