using Discord;
using Discord.Interactions;

namespace DiscordApp.Justice.Commands
{
    public class Bizness : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("bizness-embed", "Отправляет сообщение с кнопками для регистрации")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task sendBiznessEmbed()
        {
            await DeferAsync(true);
            var Embed = new EmbedBuilder()
                .WithTitle("**Регистрация бизнеса!**")
                .WithDescription("Ниже вы можете нажать на кнопку для создания ИП или ООО!")
                .WithColor(Color.Blue)
                .Build();
            var Components = new ComponentBuilder()
                .WithButton(new ButtonBuilder() { CustomId = "NewIndividualEntrepreneur", Label = "ИП", Style = ButtonStyle.Primary })
                .WithButton(new ButtonBuilder() { CustomId = "NewBizness", Label = "ООО", Style = ButtonStyle.Primary })
                .Build();
            await Context.Channel.SendMessageAsync(embed: Embed, components: Components);
            await FollowupAsync("OK!");//, ephemeral: true);
        }
        
    }
}
