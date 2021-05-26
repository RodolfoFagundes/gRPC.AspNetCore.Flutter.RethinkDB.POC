using System.Threading;
using System.Threading.Tasks;

namespace GrpcServer.Db.Abstractions
{
    public interface IChatChangefeedDbService
    {
        Task EnsureDatabaseCreatedAsync();

        Task InsertChatAsync(ChatModel chatModel);

        Task<IChangefeed<ChatModel>> GetChatChangefeedAsync(CancellationToken cancellationToken);
    }
}
