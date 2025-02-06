using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.Services.Interfaces
{
    public interface IRequestor
    {
        Task<RestResponse> PostRequest(string svc, string parameters);
        void AddAuth(string authToken);
    }
}
