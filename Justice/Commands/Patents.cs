using Discord.Interactions;
using Discord;

namespace DiscordApp.Justice.Commands
{
    public class Patents : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }

        [SlashCommand("send_patent_embed", description: "Отправляет сообщение для регистрации паспортов")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task sendPatentBuilerEmbed()
        {
            await DeferAsync(true);
            var Embed = new EmbedBuilder()
                .WithTitle("**Регистрация патента!**")
                .WithDescription("Ниже вы можете нажать на кнопку для создания патентов!")
                .WithColor(Color.Blue)
                .Build();
            var Components = new ComponentBuilder()
                .WithButton(new ButtonBuilder() { CustomId = "artPatent", Label = "Патент на арт", Style = ButtonStyle.Primary })
                .WithButton(new ButtonBuilder() { CustomId = "bookPatent", Label = "Патент на книгу", Style = ButtonStyle.Primary })
                .Build();
            await Context.Channel.SendMessageAsync(embed: Embed, components: Components);
            await FollowupAsync("OK!");
        }

    }
}
