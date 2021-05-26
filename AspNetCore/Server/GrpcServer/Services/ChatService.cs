using Chat;
using Grpc.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcServer
{
    internal class ChatService : ChatRoom.ChatRoomBase
    {
        private readonly Room _chatRoomService;
        private readonly ChatRepository _chatRepository;

        public ChatService(Room chatRoomService, ChatRepository chatRepository)
        {
            _chatRoomService = chatRoomService;
            _chatRepository = chatRepository;
        }

        public override async Task join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;

            do
            {
                _chatRoomService.Join(requestStream.Current.User, responseStream);

                _ = _chatRepository.ExecuteInsertAsync(
                    new Db.Abstractions.ChatModel
                    { User = requestStream.Current.User, Content = requestStream.Current.Text, DateTimeChange = DateTime.Now },
                    CancellationToken.None);

                await _chatRoomService.BroadcastMessageAsync(requestStream.Current);
            } while (await requestStream.MoveNext());

            _chatRoomService.Remove(context.Peer);

            WaitHandle.WaitAny(new[] { context.CancellationToken.WaitHandle });
        }
    }
}