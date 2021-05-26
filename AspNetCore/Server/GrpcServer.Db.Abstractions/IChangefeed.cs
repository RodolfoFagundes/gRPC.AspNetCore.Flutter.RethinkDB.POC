using System.Collections.Generic;
using System.Threading;

namespace GrpcServer.Db.Abstractions
{
    public interface IChangefeed<out T>
    {
        IAsyncEnumerable<T> FetchFeed(CancellationToken cancellationToken = default);
    }
}
