using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.Models.UnitGroups
{
    public class UnitGroup
    {
        public string nm { get; set; }
        public Int64 cls { get; set; }
        public Int64 id { get; set; }
        public Int64 mu { get; set; }
        public List<long> u { get; set; }
        public Int64 uacl { get; set; }
    }
}
