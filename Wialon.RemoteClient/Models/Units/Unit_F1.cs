using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.Models.Units
{
    public class Unit_F1
    {
        /* measure units: 0 - si, 1 - us, 2 - imperial, 3 - metric with gallons */
        public int mu { get; set; }
        
        /* name */
        public string nm { get; set; }
        
        /* superclass ID: "avl_unit" */
        public int cls { get; set; }
        
        /* unit ID */
        public Int64 id { get; set; }

        /* current user access level for unit */
        public Int64 uacl { get; set; }
    }
}
