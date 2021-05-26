using Chat;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Name");
            var userName = Console.ReadLine();

            // ***** IMPORTANTE *****
            // Ativa suporte para HTTP2 não criptografado, necessário para o uso de HTTP
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Insecure
            };

            using var channel = GrpcChannel.ForAddress("http://192.168.1.198:5001", grpcChannelOptions);

            var client = new ChatRoom.ChatRoomClient(channel);

            using (var chat = client.join())
            {
                _ = Task.Run(async () =>
                {
                    while (await chat.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
                    {
                        var response = chat.ResponseStream.Current;
                        Console.WriteLine($"{response.User}: {response.Text}");
                    }
                });

                await chat.RequestStream.WriteAsync(new Message { User = userName, Text = $"{userName} has joined the room" });

                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    if (line.ToLower() == "bye")
                    {
                        break;
                    }
                    await chat.RequestStream.WriteAsync(new Message { User = userName, Text = line });
                }
                await chat.RequestStream.CompleteAsync();
            }

            Console.WriteLine("Disconnecting");
            await channel.ShutdownAsync();
        }
    }
}
