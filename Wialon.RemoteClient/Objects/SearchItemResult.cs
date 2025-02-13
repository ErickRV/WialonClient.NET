using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.DTOs.Error;

namespace Wialon.RemoteClient.Objects
{
    public class SearchItemResult<T>
    {

        public T item { get; set; }
        public UInt64 flags { get; set; }

        public bool Success { get; set; } = true;
        public ErrorDto Error { get; set; }
    }
}
