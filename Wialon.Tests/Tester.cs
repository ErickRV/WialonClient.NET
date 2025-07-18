using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Core;
using Wialon.RemoteClient.DTOs.LogIn;
using Wialon.RemoteClient.Models.Geofence;
using Wialon.RemoteClient.Models.UnitGroups;
using Wialon.RemoteClient.Models.Units;
using Wialon.RemoteClient.Objects;
using Wialon.RemoteClient.Services;
using Wialon.Tests.Utils;

namespace Wialon.Tests
{
    [TestClass]
    public class Tester
    {
        //[TestMethod]
        public async Task RunTest() {

            string[] config = File.ReadAllText("../../../conf.txt").Split('@');
            string host = config[0];
            string token = config[1];

            WialonClient wialonClient = new WialonClient(host, token);
            LogInResult logInResult = await wialonClient.LogIn();

            SearchItemsParams allgroups = SearchParamsFactory.AllUnitGroups(1);
            SearchItemsResult<UnitGroup> resultGroups = await wialonClient.SearchItems<UnitGroup>(allgroups);
            List<UnitGroup> unitGroupSectors = resultGroups.items.Where(x => x.nm.Contains("Sector")).ToList();

            UnitGroup sector4 = unitGroupSectors.First(x => x.nm == "Sector 4");
            UnitGroup sector2 = unitGroupSectors.First(x => x.nm == "Sector 2");

            long[] insertTest = [];
            object x = new { itemId = sector4.id, units = insertTest };

            string result = await wialonClient.RawRequest("unit_group/update_units", x);


            
            
            
            //SearchItemsParams searchAllZones = SearchParamsFactory.AllZones();
            //var result = await wialonClient.SearchItems<GeoFenceGroup>(searchAllZones);
            //GeoFenceGroup geoFenceGroup = result.items.First();

            //Dictionary<string, GeoFence> geoFences = geoFenceGroup.zl.Where(x => x.Value.n.Contains("SECTOR")).ToDictionary();
            //geoFenceGroup.zl = geoFences;

            //Dictionary<string, long[]> pointInGeoF = await wialonClient.GeofencesByPoint(20.986225, -101.479373, geoFenceGroup);
        }
    }
}
