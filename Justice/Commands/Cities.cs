using Discord.Interactions;
using Discord;

namespace DiscordApp.Justice.Commands
{
    public class Cities : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("cities-embed", "Отправляет сообщение с кнопками для регистрации")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task sendCityEmbed()
        {
            await DeferAsync(true);
            var Embed = new EmbedBuilder()
                .WithTitle("**Регистрация базы на карту!**")
                .WithDescription("Ниже вы можете нажать на кнопку для подачи заявки на добавления базы на карту!")
                .WithColor(Color.Blue)
                .Build();
            var Components = new ComponentBuilder()
                .WithButton(new ButtonBuilder() { CustomId = "addBaseOnMap", Label = "Добавить", Style = ButtonStyle.Success})
                .Build();
            await Context.Channel.SendMessageAsync(embed: Embed, components: Components);
            await FollowupAsync("OK!");//, ephemeral: true);
        }

    }
}
