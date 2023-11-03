using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp.Database;
using Microsoft.VisualBasic;
using System.Reflection;


namespace DiscordApp
{
    public class JusticeHandler
    {
        private readonly DiscordSocketClient client;
        private readonly InteractionService handler;
        private readonly IServiceProvider services;
        private readonly IConfiguration configuration;

        public JusticeHandler(DiscordSocketClient client, InteractionService handler, IConfiguration config)
        {
            this.client = client;
            this.handler = handler;
            this.services = Startup.serviceProvider;
            this.configuration = config;
        }

        public async Task InitializeAsync()
        {
            client.Ready += ReadyAsync;
            handler.Log += LogAsync;

            await handler.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            var guildCommand = new SlashCommandBuilder();

            client.InteractionCreated += HandleInteraction;
        }

        private async Task LogAsync(LogMessage log)
            => Console.WriteLine(log);

        private async Task ReadyAsync()
        {
            await client.SetGameAsync("yaflay.ru", "https://yaflay.ru/", ActivityType.Watching);
            await handler.RegisterCommandsGloballyAsync(true);
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(client, interaction);
                //await context.Interaction.DeferAsync(true);
                var result = await handler.ExecuteCommandAsync(context, services);

                if (!result.IsSuccess)
                    await interaction.RespondAsync($"Возникла какая-то ошибка: {result.Error}", ephemeral: true);
            }
            catch
            {
                if (interaction.Type is InteractionType.ApplicationCommand)
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());  ;
            }
        }
    }
}
