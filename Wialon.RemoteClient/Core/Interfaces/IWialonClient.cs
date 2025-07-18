using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Models.Geofence;
using Wialon.RemoteClient.Objects;

namespace Wialon.RemoteClient.Core.Interfaces
{
    public interface IWialonClient
    {
        public Task<LogInResult> LogIn();
        public Task<SearchItemResult<T>> SearchItem<T>(Int64 id, Int64 flags);
        public Task<SearchItemsResult<T>> SearchItems<T>(SearchItemsParams searchParams);
        public Task<Dictionary<string, long[]>> GeofencesByPoint(double laitude, double longitude, GeoFenceGroup geoFenceGroup);

        public Task<string> RawRequest(string svc, object parameters);
    }
}
