using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wialon.RemoteClient.Models.Units
{
    public class Unit_F4194304
    {
        /// <summary>
        /// Object containing the last kown position
        /// </summary>
        public Position pos { get; set; }
    }

    public class Position {
        /// <summary>
        /// time (UTC)
        /// </summary>
        public uint t { get; set; }

        /// <summary>
        /// latitude 
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// longitude
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// altitude 
        /// </summary>
        public double z { get; set; }

        /// <summary>
        /// speed 
        /// </summary>
        public int s { get; set; }

        /// <summary>
        /// course
        /// </summary>
        public int c { get; set; }

        /// <summary>
        /// satellites count
        /// </summary>
        public int sc { get; set; }
    }
}
