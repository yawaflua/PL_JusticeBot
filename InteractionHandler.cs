﻿using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp.Database;
using System.Reflection;


namespace DiscordApp
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient client;
        private readonly InteractionService handler;
        private readonly IServiceProvider services;
        private readonly IConfiguration configuration;

        public InteractionHandler(DiscordSocketClient client, InteractionService handler, IConfiguration config)
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

            client.InteractionCreated += HandleInteraction;
        }

        private async Task LogAsync(LogMessage log)
            => Console.WriteLine(log);

        private async Task ReadyAsync()
        {

            await handler.RegisterCommandsGloballyAsync(true);

        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(client, interaction);

                var result = await handler.ExecuteCommandAsync(context, services);

                if (!result.IsSuccess)
                    switch (result.Error)
                    {
                        case InteractionCommandError.UnmetPrecondition:
                            break;
                        default:
                            break;
                    }
            }
            catch
            {
                if (interaction.Type is InteractionType.ApplicationCommand)
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}
