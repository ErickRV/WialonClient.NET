using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Core;
using Wialon.RemoteClient.Objects;

namespace Wialon.Tests.Core
{
    [TestClass]
    public class SearchParamsFactoryTests
    {
        [TestMethod]
        public void ByProfileField_Value() {
            //Arrange 
            string val = Guid.NewGuid().ToString();
            Int64 flags = 4611686018427387903;

            //Act
            SearchItemsParams searchParams = SearchParamsFactory.Unit_ByProfileField_Value(val, flags);

            //Assert
            Assert.AreEqual("avl_unit", searchParams.spec.itemsType);
            Assert.AreEqual("rel_profilefield_value", searchParams.spec.propName);
            Assert.AreEqual(val, searchParams.spec.propValueMask);
            Assert.AreEqual("rel_profilefield_value", searchParams.spec.sortType);

            Assert.AreEqual(0, searchParams.force);
            Assert.AreEqual(flags, searchParams.flags);
            Assert.AreEqual(0, searchParams.from);
            Assert.AreEqual(0, searchParams.to);
        }
    }
}
