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

            SearchItemResult<Unit_F4194304> unit = await wialonClient.SearchItem<Unit_F4194304>(401655754, 4194304);
        }
    }
}
