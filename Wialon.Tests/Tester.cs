using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Core;
using Wialon.RemoteClient.DTOs.LogIn;
using Wialon.RemoteClient.Models.Units;
using Wialon.RemoteClient.Objects;
using Wialon.RemoteClient.Services;

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

            SearchItemsParams searchItemsParams = SearchParamsFactory.Unit_ByProfileField_Value("LMWDT1G84R1148796", 1);
            SearchItemsResult<Unit_F1> result = await wialonClient.SearchItems<Unit_F1>(searchItemsParams);
        }
    }
}
