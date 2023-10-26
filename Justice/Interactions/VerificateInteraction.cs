using Discord.Interactions;

namespace DiscordApp.Justice.Interactions
{
    public class VerificateInteraction : InteractionModuleBase<SocketInteractionContext>
    {
        [ComponentInteraction("UserVerification")]
        public async Task userVerificationInteraction()
        {
            await DeferAsync(true);
            try
            {
                var user = await Startup.sp.GetUser(Context.User.Id.ToString());
                if (user.IsPlayer())
                {
                    await FollowupAsync("Готово!", ephemeral: true);
                    var guildUser = Context.Guild.GetUser(Context.User.Id);
                    await guildUser.AddRoleAsync(1165687128366268511);
                    await guildUser.ModifyAsync(func =>
                    {
                        func.Nickname = user.Name;
                    });

                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"User {Context.User.GlobalName} not found! Error: {ex.Message}");
                await FollowupAsync("Какая-то ошибка, возможно вы не игрок пупленда...", ephemeral: true);
            }
        }
    }
}
