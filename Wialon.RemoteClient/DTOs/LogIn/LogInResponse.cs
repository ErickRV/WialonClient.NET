using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.DTOs.LogIn
{
    public class LogInResponse
    {
        public string host { get; set; }
        public string eid { get; set; }
        public string gis_sid { get; set; }
        public string au { get; set; }
        public string tm { get; set; }
    }
}
