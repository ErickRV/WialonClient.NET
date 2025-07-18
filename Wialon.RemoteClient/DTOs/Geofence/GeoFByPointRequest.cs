using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.DTOs.Geofence
{
    public class GeoFByPointRequest
    {
        public GeoFByPointSpec spec { get; set; }
    }

    public class GeoFByPointSpec {
        public double lat { get; set; }
        public double lon { get; set; }
        public double radius { get; set; }
        public Dictionary<string, long[]> zoneId { get; set; }
    }
}
