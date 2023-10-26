using Discord;
using Discord.WebSocket;
using DiscordApp;

namespace DiscordApp.justice
{
    public class SelfBotService : BackgroundService
    {
        private readonly DiscordSocketClient client;

        public SelfBotService(
            DiscordSocketClient client
            )
        {
            this.client = client;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            client.Log += LogAsync;
            await client.LoginAsync(TokenType.Bearer, "MTAyNDA2MzQ2MzQ4NTYwNzk3Nw.G7rg3W.NiVYFH9x8V2y2eKP7b9XP5th7Of1XhF2WFVF_M");
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        public async Task<IGuildUser> GetUser(string name)
        {
            var guild = client.GetGuild(995379037407027270);
            var users = await guild.GetUsersAsync().FlattenAsync();
            foreach (IGuildUser user in users)
            {
                if (user.DisplayName == name)
                {
                    return user;
                }
            }
            return null;
        }

        private async Task LogAsync(LogMessage message)
            => Console.WriteLine(message.ToString());
    }
}
