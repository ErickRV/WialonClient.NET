using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Core;
using Wialon.RemoteClient.DTOs.LogIn;
using Wialon.RemoteClient.Models.Geofence;
using Wialon.RemoteClient.Models.Units;
using Wialon.RemoteClient.Objects;
using Wialon.RemoteClient.Services;
using Wialon.Tests.Utils;

namespace Wialon.Tests
{
    [TestClass]
    public class Tester
    {
        [TestMethod]
        public async Task RunTest() {

            string[] config = File.ReadAllText("../../../conf.txt").Split('@');
            string host = config[0];
            string token = config[1];

            WialonClient wialonClient = new WialonClient(host, token);
            LogInResult logInResult = await wialonClient.LogIn();

            //SearchItemsParams allgroups = SearchParamsFactory.AllUnitGroups(1);
            //SearchItemsResult<object> resultGroups = await wialonClient.SearchItems<object>(allgroups);
            //string groups = JsonConvert.SerializeObject(resultGroups.items);


            //SearchItemsParams allZones = SearchParamsFactory.AllZones(0x00001001);
            SearchItemsParams allZones = SearchParamsFactory.AllZones();
            var result = await wialonClient.SearchItems<GeoFenceGroup>(allZones);
            GeoFenceGroup geoFenceGroup = result.items.First();

            List<GeoFence> geoFences = geoFenceGroup.zl.Where(x => x.Value.n.Contains("SECTOR")).Select(x => x.Value).ToList();
            long[] GeoFenceIds = geoFences.Select(x => x.id).ToArray();

            Dictionary<string, long[]> resurceGeoFences = new Dictionary<string, long[]>
            {
                { geoFenceGroup.id.ToString(), GeoFenceIds }
            };
            object x = new
            {
                spec = new pointSearch
                {
                    lat = 31.861007,
                    lon = -116.623453,
                    radius = 0,
                    zoneId = resurceGeoFences
                }
            };
            object zbypoint = await wialonClient.RawRequest("resource/get_zones_by_point", x);
            string test = JsonConvert.SerializeObject(zbypoint);
            
        }
    }

    public class pointSearch {
        public double lat { get; set; }
        public double lon { get; set; }
        public double radius { get; set; }
        public Dictionary<string, long[]> zoneId { get; set; }
    }
}
