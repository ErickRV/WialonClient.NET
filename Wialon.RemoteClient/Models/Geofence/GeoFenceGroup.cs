using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Models.Resources;

namespace Wialon.RemoteClient.Models.Geofence
{
    public class GeoFenceGroup : Resource
    {
        /// <summary>
        /// the key in the dictionary is the GeoFence Id
        /// </summary>
        public Dictionary<string, GeoFence> zl { get; set; } = new Dictionary<string, GeoFence>();
    }
}
