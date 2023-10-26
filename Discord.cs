using Discord;
using Discord.WebSocket;


namespace DiscordApp
{
    public class DiscordBotService : BackgroundService
    {
        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private readonly DiscordSocketClient client;
        private readonly IConfiguration configuration;
        private readonly InteractionHandler interactionHandler;

        public DiscordBotService(
            IHostApplicationLifetime hostApplicationLifetime,
            DiscordSocketClient client,
            IConfiguration configuration,
            InteractionHandler interactionHandler)
        {
            this.interactionHandler = interactionHandler;
            this.configuration = configuration;
            this.client = client;
            this.hostApplicationLifetime = hostApplicationLifetime;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            client.Log += LogAsync;

            await client.LoginAsync(TokenType.Bot, "MTE1Mjk0Njg2NTMwNjg4MjExOA.GBp_Wv.MacxgnAz_idgNj7du8hrkOxuWfoyuVpj9zPsM4");
            await client.StartAsync();
            await interactionHandler.InitializeAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private async Task LogAsync(LogMessage message)
            => Console.WriteLine(message.ToString());
    }
}
