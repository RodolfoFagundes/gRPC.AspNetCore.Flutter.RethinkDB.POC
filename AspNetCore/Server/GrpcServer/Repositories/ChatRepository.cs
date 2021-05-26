using GrpcServer.Db.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcServer
{
    internal class ChatRepository
    {
        private readonly IChatChangefeedDbService _chatChangefeedDbService;

        public ChatRepository(IChatChangefeedDbService chatChangefeedDbService)
        {
            _chatChangefeedDbService = chatChangefeedDbService;
        }

        public async Task ExecuteInsertAsync(ChatModel chatModel, CancellationToken stoppingToken)
        {
            await _chatChangefeedDbService.EnsureDatabaseCreatedAsync();

            if (!stoppingToken.IsCancellationRequested)
                await _chatChangefeedDbService.InsertChatAsync(chatModel);
        }
    }
}
