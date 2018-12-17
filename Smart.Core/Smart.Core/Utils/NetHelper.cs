using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Smart.Core.Utils
{
    public static class NetHelper
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<string, HttpClient> _dichttp = new System.Collections.Concurrent.ConcurrentDictionary<string, HttpClient>();
        public static HttpClient GetHttpClient(string gamecode)
        {
            if (_dichttp.ContainsKey(gamecode))
            {
                return _dichttp[gamecode];
            }
            else
            {
                HttpClient _client = new HttpClient();
                _client.Timeout = new TimeSpan(0, 0, 10);
                _client.DefaultRequestHeaders.Connection.Add("keep-alive");
                _dichttp[gamecode] = _client;
                return _client;
            }

        }
    }
}
