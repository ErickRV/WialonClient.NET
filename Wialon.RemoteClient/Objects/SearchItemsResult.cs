using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.DTOs.Error;

namespace Wialon.RemoteClient.Objects
{
    public class SearchItemsResult<T>
    {
        public Spec searchSpec { get; set; }
        public int dataFlags { get; set; }
        public int totalItemsCount { get; set; }
        public int indexFrom { get; set; }
        public int indexTo { get; set; }
        public List<T> items { get; set; }

        public bool Success { get; set; } = true;
        public ErrorDto Error { get; set; }
    }
}
