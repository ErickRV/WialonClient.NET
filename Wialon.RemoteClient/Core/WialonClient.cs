using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Core.Interfaces;
using Wialon.RemoteClient.DTOs.Error;
using Wialon.RemoteClient.DTOs.Geofence;
using Wialon.RemoteClient.DTOs.LogIn;
using Wialon.RemoteClient.Models.Geofence;
using Wialon.RemoteClient.Models.Units;
using Wialon.RemoteClient.Objects;
using Wialon.RemoteClient.Services;
using Wialon.RemoteClient.Services.Interfaces;

namespace Wialon.RemoteClient.Core
{
    public class WialonClient : IWialonClient
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

        public async Task<SearchItemResult<T>> SearchItem<T>(Int64 id, Int64 flags)
        {
            string dto = JsonConvert.SerializeObject(new  { id, flags });
            RestResponse restResponse = await requestor.PostRequest("core/search_item", dto);
            if (restResponse.Content.Contains("\"error\":"))
            {
                ErrorDto errorDto = JsonConvert.DeserializeObject<ErrorDto>(restResponse.Content);
                return new SearchItemResult<T> { Success = false, Error = errorDto };
            }

            return JsonConvert.DeserializeObject<SearchItemResult<T>>(restResponse.Content);
        }

        /// <summary>
        /// Wialon Search result will always return a list of results so when setting the type of the result
        /// consider it will already deserialize as a list of that type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public async Task<SearchItemsResult<T>> SearchItems<T>(SearchItemsParams searchParams) {
            RestResponse restResponse = await requestor.PostRequest("core/search_items", JsonConvert.SerializeObject(searchParams));
            if (restResponse.Content.Contains("\"error\":")) {
                ErrorDto errorDto = JsonConvert.DeserializeObject<ErrorDto>(restResponse.Content);
                return new SearchItemsResult<T> { Success = false, Error = errorDto};
            }
                return JsonConvert.DeserializeObject<SearchItemsResult<T>>(restResponse.Content);
        }

        public async Task<Dictionary<string, long[]>> GeofencesByPoint(double laitude, double longitude, GeoFenceGroup geoFenceGroup)
        {
            GeoFByPointRequest GeoByPReq = new GeoFByPointRequest
            {
                spec = new GeoFByPointSpec { 
                    lat = laitude,
                    lon = longitude,
                    radius = 0,
                    zoneId = new Dictionary<string, long[]> {
                        { geoFenceGroup.id.ToString(), geoFenceGroup.zl.Select(x => x.Value.id).ToArray() }
                    }
                }
            };
            
            RestResponse restResponse = await requestor.PostRequest("resource/get_zones_by_point", JsonConvert.SerializeObject(GeoByPReq));
            if (restResponse.Content.Contains("\"error\":"))
                throw new ApplicationException("Api returned error!", new Exception(restResponse.Content));

            return JsonConvert.DeserializeObject<Dictionary<string, long[]>>(restResponse.Content);
        }



        /// <summary>
        /// This method can be used to send any request using this authenticated wialon client
        /// </summary>
        /// <param name="endpoint">this route will be used in the svc part of the request</param>
        /// <param name="parameters">parameters will be serialized and used in the params part of the request </param>
        /// <returns>Returns the content of the server response as a string</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> RawRequest(string endpoint, object parameters)
        {
            RestResponse restResponse = await requestor.PostRequest(endpoint, JsonConvert.SerializeObject(parameters));
            return restResponse.Content;
        }
    }

}
