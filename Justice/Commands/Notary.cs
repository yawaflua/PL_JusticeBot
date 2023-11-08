using Discord.Interactions;
using Discord;

namespace DiscordApp.Justice.Commands
{
    public class Notary : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("notary-embed", "Отправляет сообщение с кнопками для нотариусов")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task sendNotaryEmbed()
        {
            await DeferAsync(true);
            var Embed = new EmbedBuilder()
                .WithTitle("**Заверка документа**")
                .WithDescription("Ниже вы можете нажать на кнопку создания ID для документа и документа заверки")
                .WithColor(Color.Blue)
                .Build();
            var Components = new ComponentBuilder()
                .WithButton(new ButtonBuilder() { CustomId = "NewDocumentCertificate", Label = "Заверить документ", Style = ButtonStyle.Primary })
                .Build();
            await Context.Channel.SendMessageAsync(embed: Embed, components: Components);
            await FollowupAsync("OK!");//, ephemeral: true);
        }

    }
}
