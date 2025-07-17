using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Objects;

namespace Wialon.RemoteClient.Core
{
    public class SearchParamsFactory
    {
        public static SearchItemsParams Unit_ByProfileField_Value(string value, Int64 flags)
        {
            return new SearchItemsParams
            {
                spec = new Spec
                {
                    itemsType = "avl_unit",
                    propName = "rel_profilefield_value",
                    propValueMask = value,
                    sortType = "rel_profilefield_value"
                },
                force = 0,
                flags = flags,
                from = 0,
                to = 0
            };
        }

        public static SearchItemsParams AllUnitGroups(Int64 flags) { 
            return new SearchItemsParams()
            {
                spec = new Spec
                {
                    itemsType = "avl_unit_group",
                    propName = "sys_name",
                    propValueMask = "*",
                    sortType = "sys_name"
                },
                force = 0,
                flags = flags,
                from = 0,
                to = 0
            };
        }

        public static SearchItemsParams AllZones(Int64 flags = 0x00001001)
        {
            return new SearchItemsParams
            {
                spec = new Spec
                {
                    itemsType = "avl_resource",
                    propName = "zones_library",
                    propType = "propitemname",
                    propValueMask = "!",
                    sortType = "sys_name"
                },
                force = 1,
                flags = flags,
                from = 0,
                to = 0
            };
        }
    }
}
