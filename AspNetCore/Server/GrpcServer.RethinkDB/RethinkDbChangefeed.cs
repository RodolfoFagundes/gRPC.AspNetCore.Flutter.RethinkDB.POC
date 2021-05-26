using GrpcServer.Db.Abstractions;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GrpcServer.RethinkDB
{
    internal class RethinkDbChangefeed<T> : IChangefeed<T>
    {
        private readonly Cursor<Change<T>> _changefeed;

        public RethinkDbChangefeed(Cursor<Change<T>> changefeed)
        {
            _changefeed = changefeed;
        }

        public async IAsyncEnumerable<T> FetchFeed([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (await _changefeed.MoveNextAsync(cancellationToken))
            {
                yield return _changefeed.Current.NewValue;
            }
        }
    }
}
