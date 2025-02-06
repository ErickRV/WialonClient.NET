using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.Objects
{
    public class SearchItemsParams
    {
        public Spec spec { get; set; }
        public int force { get; set; }
        public Int64 flags { get; set; }
        public int from { get; set; }
        public int to { get; set; }
    }

    public class Spec {
        public string itemsType { get; set; }
        public string propName { get; set; }
        public string propValueMask { get; set; }
        public string sortType { get; set; }
        public string propType { get; set; }
        public string or_logic { get; set; }
    }
}
