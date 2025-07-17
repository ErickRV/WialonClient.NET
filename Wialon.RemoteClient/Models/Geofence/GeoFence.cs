using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.Models.Geofence
{
    public class GeoFence
    {
        public string n { get; set; }
        public string d { get; set; }
        public long id { get; set; }
        public int f { get; set; }
        public int t { get; set; }
        public int w { get; set; }
        public int e { get; set; }
        public long c { get; set; }
        public long i { get; set; }
        public long libId { get; set; }
        public string path { get; set; }
        public GeoFenceProps b { get; set; }
        public int ct { get; set; }
        public int mt { get; set; }
    }

    public class GeoFenceProps {
        public double min_x { get; set; }
        public double min_y { get; set; }
        public double max_x { get; set; }
        public double max_y { get; set; }
        public double cen_x { get; set; }
        public double cen_y { get; set; }
    }
}
