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
            client.Ready += onReady;
            await client.LoginAsync(TokenType.Bot, "MTE2NjA3OTk3NjQ0NjEwMzYwMg.GAKOIo.4af972Wh11G0EF4O5tNYb7l-vt5OwMc4HPRnjE");
            await client.StartAsync();
            await interactionHandler.InitializeAsync();

            await Task.Delay(Timeout.Infinite);
        }
        public async Task onReady()
        {
            ChangeNicknames();
            var myTimer = new System.Timers.Timer(6 * 60 * 60 * 1000); //calculate six hours in milliseconds
            myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            myTimer.Start();
        }
        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Started timer");
            await ChangeNicknames();
        }
        private async Task ChangeNicknames() 
        {
            IGuild guild = client.GetGuild(1107742957458685985);
            IRole role = guild.GetRole(1136564585420304444);
            var members = await guild.GetUsersAsync();
            var membersArray = members.ToArray();
            foreach (IGuildUser user in membersArray)
            {
                var spUser = await Startup.sp.GetUser(user.Id.ToString());
                if (spUser.IsPlayer())
                {
                    await user.ModifyAsync(func => { func.Nickname = spUser.Name; });
                    await user.AddRoleAsync(role);
                }
            }
        }
        private async Task LogAsync(LogMessage message)
            => Console.WriteLine(message.ToString());
    }
}
