using Discord;
using Discord.WebSocket;
using DiscordApp;
using System.Timers;

namespace DiscordApp
{
    public class JusticeBotService : BackgroundService
    {
        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private readonly DiscordSocketClient client;
        private readonly IConfiguration configuration;
        private readonly JusticeHandler interactionHandler;

        public JusticeBotService(
            IHostApplicationLifetime hostApplicationLifetime,
            DiscordSocketClient client,
            IConfiguration configuration,
            JusticeHandler interactionHandler)
        {
            this.interactionHandler = interactionHandler;
            this.configuration = configuration;
            this.client = client;
            this.hostApplicationLifetime = hostApplicationLifetime;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            client.Log += LogAsync;
            client.UserJoined += Justice.Events.Events.onJoinGuild;
            client.MessageReceived += Justice.Events.Events.onMessageCreate;
            //client.Ready += onReady;
            await client.LoginAsync(TokenType.Bot, "MTE2NjA3OTk3NjQ0NjEwMzYwMg.GAKOIo.4af972Wh11G0EF4O5tNYb7l-vt5OwMc4HPRnjE");
            await client.StartAsync();
            await interactionHandler.InitializeAsync();
            Startup.discordSocketClient = client;
            await Task.Delay(Timeout.Infinite);
        }

        
        private async Task LogAsync(LogMessage message)
            => Console.WriteLine(message.ToString());
    }
}
