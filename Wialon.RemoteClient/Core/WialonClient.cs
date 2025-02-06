using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.DTOs.Error;
using Wialon.RemoteClient.DTOs.LogIn;
using Wialon.RemoteClient.Objects;
using Wialon.RemoteClient.Services;
using Wialon.RemoteClient.Services.Interfaces;

namespace Wialon.RemoteClient.Core
{
    public class WialonClient
    {
        public readonly string HOST;
        public readonly string TOKEN;

        private IRequestor requestor;

        public WialonClient(string host, string token)
        {   
            HOST = host;
            TOKEN = token;
            requestor = new Requestor(HOST);
        }

        public async Task<LogInResult> LogIn()
        {
            PostLogInDto dto = new PostLogInDto { token = TOKEN };
            RestResponse restResponse = await requestor.PostRequest("token/login", JsonConvert.SerializeObject(dto));

            if (restResponse.Content.Contains("\"error\":"))
            {
                ErrorDto errorDto = JsonConvert.DeserializeObject<ErrorDto>(restResponse.Content);
                return new LogInResult {Success = false, error = errorDto.error, errorMsg = errorDto.reason};
            }

            LogInResponse logInResponse = JsonConvert.DeserializeObject<LogInResponse>(restResponse.Content);
            requestor.AddAuth(logInResponse.eid);

            return new LogInResult { Success = true, eid = logInResponse.eid};
        }

        public async Task<SearchItemsResult<T>> SearchItems<T>(SearchItemsParams searchParams) {
            RestResponse restResponse = await requestor.PostRequest("core/search_items", JsonConvert.SerializeObject(searchParams));
            if (restResponse.Content.Contains("\"error\":")) {
                ErrorDto errorDto = JsonConvert.DeserializeObject<ErrorDto>(restResponse.Content);
                return new SearchItemsResult<T> { Success = false, Error = errorDto};
            }
                return JsonConvert.DeserializeObject<SearchItemsResult<T>>(restResponse.Content);
        }
    }
}
