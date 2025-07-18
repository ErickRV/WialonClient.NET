using Bogus;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Core;
using Wialon.RemoteClient.Core.Interfaces;
using Wialon.RemoteClient.DTOs.Error;
using Wialon.RemoteClient.DTOs.Geofence;
using Wialon.RemoteClient.DTOs.LogIn;
using Wialon.RemoteClient.Models.Geofence;
using Wialon.RemoteClient.Models.Units;
using Wialon.RemoteClient.Objects;
using Wialon.RemoteClient.Services.Interfaces;
using Wialon.Tests.Utils;

namespace Wialon.Tests.Core
{
    [TestClass]
    public class WialonClientTests
    {
        Faker faker = new Faker();

        Mock<IRequestor> mockRequestor = new Mock<IRequestor>();
        IWialonClient client;

        private void seedTestingContext() {
            string token = Guid.NewGuid().ToString();
            string host = faker.Internet.Url();

            client = new WialonClient(host, token);
            FieldInfo fieldInfo = client.GetType()
                .GetField("requestor", BindingFlags.Instance | BindingFlags.NonPublic);

            fieldInfo.SetValue(client, mockRequestor.Object);
        }

        [TestMethod]
        public void CreateClient_Ok() {
            //Arrange 
            string token = faker.Random.String2(16);
            string host = faker.Internet.Url();

            //Act
            WialonClient client = new WialonClient(host, token);

            //Assert
            Assert.AreEqual(host, client.HOST);
            Assert.AreEqual(token, client.TOKEN);


            FieldInfo fieldInfo = client.GetType()
                .GetField("requestor", BindingFlags.Instance | BindingFlags.NonPublic );
            IRequestor requestor = (IRequestor)fieldInfo.GetValue(client);
            Assert.AreNotEqual(default, requestor);
        }

        [TestMethod]
        public async Task Client_LogIn_Ok() {
            //Arrange 
            seedTestingContext();

            LogInResponse logInResponse = new LogInResponse
            {
                eid = Guid.NewGuid().ToString()
            };
            RestResponse restResponse = new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(logInResponse)
            };

            mockRequestor.Setup(r => r.AddAuth(It.IsAny<string>()));
            mockRequestor.Setup(r => r.PostRequest("token/login", It.IsAny<string>()))
                .Returns(Task.FromResult(restResponse));

            //Act
            LogInResult result = await client.LogIn();

            //Assert
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(logInResponse.eid, result.eid);
            Assert.AreEqual(0, result.error);
            Assert.AreEqual("No Error Description", result.errorMsg);

            mockRequestor.Verify(r => r.PostRequest("token/login", It.Is<string>(x => 
            x.Equals(JsonConvert.SerializeObject(new PostLogInDto { token = ((WialonClient)client).TOKEN })))), Times.Once);

            mockRequestor.Verify(r => r.AddAuth(It.Is<string>(x => x.Equals(logInResponse.eid))), Times.Once);
        }

        [TestMethod]
        public async Task Client_LogIn_Fails() {
            //Arrange 
            seedTestingContext();

            int errorCode = faker.Random.Int(0, 16);
            string errorMsg = faker.Random.String2(24);

            RestResponse restResponse = new RestResponse 
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(new ErrorDto { error = errorCode, reason = errorMsg })
            };

            mockRequestor.Setup(r => r.AddAuth(It.IsAny<string>()));
            mockRequestor.Setup(r => r.PostRequest("token/login", It.IsAny<string>()))
                .Returns(Task.FromResult(restResponse));

            //Act
            LogInResult result = await client.LogIn();

            //Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(errorCode, result.error);
            Assert.AreEqual(errorMsg, result.errorMsg);

            mockRequestor.Verify(r => r.AddAuth(It.IsAny<string>()), Times.Never);
            mockRequestor.Verify(r => r.PostRequest("token/login", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task SearchItems_Ok() {
            //Arrange 
            seedTestingContext();
            SearchItemsResult<Unit_F1> searchResult = new SearchItemsResult<Unit_F1>
            {
                items = new List<Unit_F1> { 
                    MockDataGenerator.GenUnit_F1(),
                    MockDataGenerator.GenUnit_F1(),
                    MockDataGenerator.GenUnit_F1(),
                },
                dataFlags = 1,
                totalItemsCount = 3,
                indexFrom = 0,
                indexTo = 2
            };
            RestResponse restResponse = new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(searchResult)
            };

            mockRequestor.Setup(r => r.PostRequest("core/search_items", It.IsAny<string>()))
                .Returns(Task.FromResult(restResponse));

            //Act
            SearchItemsParams searchParams = new SearchItemsParams
            {
                spec = new Spec {
                    itemsType = "avl_unit",
                    propName = "sys_name",
                    propValueMask = "*",
                    sortType = "sys_name"
                },
                flags = 1,
                force = 0
            };
            SearchItemsResult<Unit_F1> result = await client.SearchItems<Unit_F1>(searchParams);

            //Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(default, result.Error);
            Assert.IsTrue(TestUtils.AreObjectsEqual(result.items, searchResult.items));

            mockRequestor.Verify(r => r.PostRequest("core/search_items", It.Is<string>(s =>
            TestUtils.AreObjectsEqual(s, JsonConvert.SerializeObject(searchParams)))), Times.Once);
        }

        [TestMethod]
        public async Task SearchItems_Fails() {
            //Arrange 
            seedTestingContext();
            ErrorDto errorDto = new ErrorDto { error = 1 };
            RestResponse restResponse = new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(errorDto)
            };

            mockRequestor.Setup(r => r.PostRequest("core/search_items", It.IsAny<string>()))
                .Returns(Task.FromResult(restResponse));

            //Act 
            SearchItemsParams searchParams = new SearchItemsParams()
            {
                spec = new Spec
                {
                    itemsType = "avl_unit",
                    propName = "sys_name",
                    propValueMask = "SOME ERROR!",
                    sortType = "sys_name"
                },
                flags = 1,
                force = 0
            };
            SearchItemsResult<Unit_F1> result = await client.SearchItems<Unit_F1>(searchParams);

            //Assert
            Assert.IsFalse(result.Success);
            Assert.IsTrue(TestUtils.AreObjectsEqual(result.Error, errorDto));

            mockRequestor.Verify(r => r.PostRequest("core/search_items", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task SearchItem_Ok() {
            //Arrange 
            seedTestingContext();
            SearchItemResult<Unit_F4194304> searchItemResult = new SearchItemResult<Unit_F4194304>
            {
                item = MockDataGenerator.GenUnit_F4194304(),
                flags = 4194304
            };
            RestResponse restResponse = new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(searchItemResult)
            };

            mockRequestor.Setup(r => r.PostRequest("core/search_item", It.IsAny<string>()))
                .Returns(Task.FromResult(restResponse));

            //Act
            int id = faker.Random.Int(1);
            SearchItemResult<Unit_F4194304> result = await client.SearchItem<Unit_F4194304>(id, 4194304);

            //Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(default, result.Error);
            Assert.IsTrue(TestUtils.AreObjectsEqual(searchItemResult.item, result.item));

            string verificacion = JsonConvert.SerializeObject(new { id = id, flags = 4194304 });
            mockRequestor.Verify(r => r.PostRequest("core/search_item", It.Is<string>(x => 
            x.Equals(verificacion))), Times.Once);
        }

        [TestMethod]
        public async Task SearchItem_Fails() {
            //Arrange 
            seedTestingContext();
            ErrorDto errorDto = new ErrorDto { error = 1 };
            RestResponse restResponse = new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(errorDto)
            };

            mockRequestor.Setup(r => r.PostRequest("core/search_item", It.IsAny<string>()))
                .Returns(Task.FromResult(restResponse));

            //Act
            int id = faker.Random.Int(1);
            int randFlag = faker.Random.Int(1);
            SearchItemResult<Unit_F4194304> result = await client.SearchItem<Unit_F4194304>(id, randFlag);

            //Assert
            Assert.IsFalse(result.Success);
            Assert.IsTrue(TestUtils.AreObjectsEqual(result.Error, errorDto));

            string verificacion = JsonConvert.SerializeObject(new { id = id, flags = randFlag });
            mockRequestor.Verify(r => r.PostRequest("core/search_item", It.Is<string>(x =>
            x.Equals(verificacion))), Times.Once);
        }

        [TestMethod]
        public async Task RawRequest_Ok()
        {
            //Arrange 
            seedTestingContext();
            string endpoint = faker.Internet.Url();
            object parameters = new
            {
                id = 09379,
                test = "Test",
                faker = faker.Random.Hash()
            };

            object operationResult = new
            {
                success = true,
                message = "Success or error or something",
                data = new { some =  "data" }
            };
            RestResponse restResponse = new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(operationResult)
            };

            mockRequestor.Setup(x => x.PostRequest(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(restResponse));

            //Act
            string result = await client.RawRequest(endpoint, parameters);

            //Assert
            Assert.AreEqual(JsonConvert.SerializeObject(operationResult), result);

            mockRequestor.Verify(x => x.PostRequest(endpoint, It.Is<string>(x => 
            x.Equals(JsonConvert.SerializeObject(parameters)))), Times.Once);

        }

        [TestMethod]
        public async Task GeofencesByPoint() {
            //Arrange 
            seedTestingContext();
            double latitude = faker.Random.Double();
            double longitude = faker.Random.Double();

            GeoFence geoF1 = MockDataGenerator.GenGeoFence();
            GeoFence geoF2 = MockDataGenerator.GenGeoFence();
            GeoFence geoF3 = MockDataGenerator.GenGeoFence();

            GeoFenceGroup geoFenceGroup = new GeoFenceGroup();
            geoFenceGroup.zl.Add(geoF1.id.ToString(), geoF1);
            geoFenceGroup.zl.Add(geoF2.id.ToString(), geoF2);
            geoFenceGroup.zl.Add(geoF3.id.ToString(), geoF3);

            int randSelect = new Random().Next(0, 2);
            RestResponse restResponse = new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(new Dictionary<string, long[]> {
                    {geoFenceGroup.id.ToString(), [ geoFenceGroup.zl.ElementAt(randSelect).Value.id ] }
                })
            };

            mockRequestor.Setup(x => x.PostRequest(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(restResponse));

            //Act
            Dictionary<string, long[]> result = await client.GeofencesByPoint(latitude, longitude, geoFenceGroup);

            //Assert
            GeoFByPointRequest assertDto = new GeoFByPointRequest
            {
                spec = new GeoFByPointSpec { 
                    lat = latitude,
                    lon = longitude,
                    radius = 0,
                    zoneId = new Dictionary<string, long[]> { 
                        { geoFenceGroup.id.ToString(), geoFenceGroup.zl.Select(x => x.Value.id).ToArray() } 
                    }
                }
            };
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(geoFenceGroup.id.ToString(), result.First().Key);

            Assert.IsTrue(result.First().Value.Length == 1);
            Assert.AreEqual(result.First().Value[0], geoFenceGroup.zl.ElementAt(randSelect).Value.id);

            mockRequestor.Verify(x => x.PostRequest("resource/get_zones_by_point", It.Is<string>(x => 
            x.Equals(JsonConvert.SerializeObject(assertDto)))), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public async Task GeofencesByPoint_Fails() {
            //Arrange 
            seedTestingContext();
            ErrorDto errorDto = new ErrorDto { error = 1 };
            RestResponse restResponse = new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(errorDto)
            };

            mockRequestor.Setup(r => r.PostRequest("resource/get_zones_by_point", It.IsAny<string>()))
                .Returns(Task.FromResult(restResponse));

            //Act
            GeoFenceGroup geoFenceGroup = new GeoFenceGroup();
            GeoFence geoF1 = MockDataGenerator.GenGeoFence();
            geoFenceGroup.zl.Add(geoF1.id.ToString(), geoF1);

            Dictionary<string, long[]> result = await client.GeofencesByPoint(faker.Random.Double(), faker.Random.Double(), geoFenceGroup);
        }
    }
}
