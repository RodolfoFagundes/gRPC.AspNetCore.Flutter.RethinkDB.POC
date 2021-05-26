using GrpcServer.Db.Abstractions;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcServer
{
    internal class ChatChangefeedBackgroundService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IChatChangefeedDbService _chatChangefeedDbService;
        private readonly IServerSentEventsService _serverSentEventsService;

        public ChatChangefeedBackgroundService(ILoggerFactory loggerFactory, IChatChangefeedDbService chatChangefeedDbService, IServerSentEventsService serverSentEventsService)
        {
            _logger = loggerFactory.CreateLogger<ChatChangefeedBackgroundService>();
            _chatChangefeedDbService = chatChangefeedDbService;
            _serverSentEventsService = serverSentEventsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _chatChangefeedDbService.EnsureDatabaseCreatedAsync();

            IChangefeed<ChatModel> chatChangefeed = await _chatChangefeedDbService.GetChatChangefeedAsync(stoppingToken);

            try
            {
                await foreach (ChatModel chatModel in chatChangefeed.FetchFeed(stoppingToken))
                {
                    string newChatModel = chatModel.ToString();
                    _logger.LogInformation($"Database return: {newChatModel}");
                    await Task.WhenAll(
                        _serverSentEventsService.SendEventAsync(newChatModel)
                    );
                }
            }
            catch (OperationCanceledException)
            { }
        }
    }
}
