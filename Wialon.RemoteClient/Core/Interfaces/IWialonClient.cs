using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Objects;

namespace Wialon.RemoteClient.Core.Interfaces
{
    public interface IWialonClient
    {
        public Task<LogInResult> LogIn();
        public Task<SearchItemResult<T>> SearchItem<T>(Int64 id, Int64 flags);
        public Task<SearchItemsResult<T>> SearchItems<T>(SearchItemsParams searchParams);
    }
}
