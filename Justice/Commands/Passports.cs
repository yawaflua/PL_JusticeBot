using Discord;
using Discord.Interactions;

namespace DiscordApp.Justice.Commands
{
    public class Passports : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        [SlashCommand("send_passport_embed", description: "Отправляет сообщение для регистрации паспортов")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task sendPassportBuilerEmbed()
        {
            await DeferAsync(true);
            var Embed = new EmbedBuilder()
                .WithTitle("**Регистрация паспорта!**")
                .WithDescription("Ниже вы можете нажать на кнопку для создания темплейта паспорта!")
                .WithColor(Color.Blue)
                .Build();
            var Components = new ComponentBuilder()
                .WithButton(new ButtonBuilder() { CustomId = "newPassport", Label = "Новый паспорт", Style = ButtonStyle.Primary })
                .WithButton(new ButtonBuilder() { CustomId = "reworkPassport", Label = "Замена паспорта", Style = ButtonStyle.Primary })
                .WithButton(new ButtonBuilder() { CustomId = "BuyIdPassportButton", Label = "Покупка номера", Style = ButtonStyle.Primary })
                .Build();
            await Context.Channel.SendMessageAsync(embed: Embed, components: Components);
            await FollowupAsync("OK!");//, ephemeral: true);
        }

    }
}
