using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Services.Interfaces;

namespace Wialon.RemoteClient.Services
{
    public class Requestor : IRequestor
    {
        private string sid = string.Empty;
        private RestClient restClient;

        public Requestor(string baseUrl)
        {
            restClient = new RestClient(baseUrl);
        }

        public void AddAuth(string authToken)
        {
            sid = authToken;
        }

        public async Task<RestResponse> PostRequest(string svc, string parameters = default)
        {
            string url = $"/wialon/ajax.html?svc={svc}";
            url += $"&params={parameters}";

            if(sid != string.Empty)
                url += $"&sid={sid}";

            RestRequest restRequest = new RestRequest(url, Method.Post);
            RestResponse response = await restClient.ExecuteAsync(restRequest);

            return response;
        }
    }
}
