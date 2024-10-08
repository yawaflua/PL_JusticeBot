﻿using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp.Database;
using DiscordApp.Justice.Commands;
using DiscordApp.Types;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.Json.Nodes;

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

            HttpClient http = new HttpClient();
            
            await handler.RegisterCommandsGloballyAsync(true);
            while (true)
            {
                try
                {
                    var request = await http.GetAsync("https://api.mcsrvstat.us/3/pl.spworlds.ru");
                    var responseAboutPL = JsonConvert.DeserializeObject<ServerResponse>(request.Content.ReadAsStringAsync().Result);
                    if (!responseAboutPL.online) await client.SetGameAsync($"выключенный PL", "https://yawaflua.ru/", ActivityType.Watching);
                    else await client.SetGameAsync($"онлайн на PL: {responseAboutPL.players.online}", "https://yawaflua.ru/", ActivityType.Watching);
                }
                finally
                {
                    await Task.Delay(30000);
                }
            }

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
