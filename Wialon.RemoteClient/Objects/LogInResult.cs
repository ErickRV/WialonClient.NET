using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.Objects
{
    public class LogInResult
    {
        public bool Success { get; set; }
        public string eid { get; set; }
        public int error { get; set; }
        public string errorMsg { get; set; } = "No Error Description";
    }
}
