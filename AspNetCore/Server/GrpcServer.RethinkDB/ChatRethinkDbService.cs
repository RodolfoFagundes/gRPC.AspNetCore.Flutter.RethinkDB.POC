using GrpcServer.Db.Abstractions;
using RethinkDb.Driver.Net;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcServer.RethinkDB
{
    internal class ChatRethinkDbService : IChatChangefeedDbService
    {
        private const string DATABASE_NAME = "GrpcService";
        private const string CHAT_TABLE_NAME = "Chat";

        private readonly RethinkDb.Driver.RethinkDB _rethinkDbSingleton;
        private readonly Connection _rethinkDbConnection;

        public ChatRethinkDbService(IRethinkDbSingletonProvider rethinkDbSingletonProvider)
        {
            if (rethinkDbSingletonProvider == null)
            {
                throw new ArgumentNullException(nameof(rethinkDbSingletonProvider));
            }

            _rethinkDbSingleton = rethinkDbSingletonProvider.RethinkDbSingleton;
            _rethinkDbConnection = rethinkDbSingletonProvider.RethinkDbConnection;
        }

        public Task EnsureDatabaseCreatedAsync()
        {
            if (!((string[])_rethinkDbSingleton.DbList().Run(_rethinkDbConnection).ToObject<string[]>()).Contains(DATABASE_NAME))
            {
                _rethinkDbSingleton.DbCreate(DATABASE_NAME).Run(_rethinkDbConnection);
            }

            var database = _rethinkDbSingleton.Db(DATABASE_NAME);
            if (!((string[])database.TableList().Run(_rethinkDbConnection).ToObject<string[]>()).Contains(CHAT_TABLE_NAME))
            {
                database.TableCreate(CHAT_TABLE_NAME).Run(_rethinkDbConnection);
            }

            return Task.CompletedTask;
        }

        public Task InsertChatAsync(ChatModel chatModel)
        {
            _rethinkDbSingleton.Db(DATABASE_NAME).Table(CHAT_TABLE_NAME).Insert(chatModel).Run(_rethinkDbConnection);

            return Task.CompletedTask;
        }

        public async Task<IChangefeed<ChatModel>> GetChatChangefeedAsync(CancellationToken cancellationToken)
        {
            return new RethinkDbChangefeed<ChatModel>(
                await _rethinkDbSingleton.Db(DATABASE_NAME).Table(CHAT_TABLE_NAME).Changes().RunChangesAsync<ChatModel>(_rethinkDbConnection, cancellationToken)
            );
        }
    }
}
