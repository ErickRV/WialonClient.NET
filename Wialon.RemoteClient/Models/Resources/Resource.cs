using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.Models.Resources
{
    public abstract class Resource
    {
        public string nm { get; set; }
        public int cls { get; set; }
        public long id { get; set; }
        public int mu { get; set; }
        public long uacl { get; set; }
    }
}
