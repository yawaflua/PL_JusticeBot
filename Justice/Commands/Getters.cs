using Discord.Interactions;
using Discord;

namespace DiscordApp.Justice.Commands
{
    public class Getters : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("embed-getter-passport", "Отправляет сообщение для поиска паспорта")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task getPassport()
        {
            await DeferAsync(true);
            var embed = new EmbedBuilder()
                .WithTitle("Получение информации о паспорте")
                .WithDescription("Нажав на кнопку ниже вы можете получить данные о том, или ином игроке")
                .WithColor(Color.DarkBlue)
                .Build();
            var components = new ComponentBuilder()
                .WithButton(new ButtonBuilder()
                {
                    CustomId = "searchPassport",
                    Label = "Поиск паспорта",
                    Style = ButtonStyle.Success
                })
                .Build();
            var channel = Context.Channel as ITextChannel;
            await channel.SendMessageAsync(embed: embed, components: components);
            await FollowupAsync("Готово!", ephemeral: true);
        }
    }
}
