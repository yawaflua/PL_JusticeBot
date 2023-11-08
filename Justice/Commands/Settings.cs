using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp.Database;
using DiscordApp.Database.Tables;
using Microsoft.OpenApi.Any;
using System.Net;
using System.Text.Json.Nodes;

namespace DiscordApp.Discord.Commands
{
    public class AdminCommands : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }

        [SlashCommand("branches", "Настройка автоветок")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task addAutoBranches(IChannel channel, string branchName = "Обсуждение")
        {
            //await DeferAsync(true);
            Autobranches autobranches = new()
            {
                ChannelId = channel.Id,
                BranchName = branchName
            };
            Startup.appDbContext.Autobranches.Add(autobranches);
            await Startup.appDbContext.SaveChangesAsync();
            await FollowupAsync($"Автоветки для канала <#{channel.Id}> настроены", ephemeral: true);
        }


        [SlashCommand("embed", "Отправить эмбед")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task sendAsEmbed(string description, string? title = null, string? footer = null, IAttachment? attachment = null)
        {
            var author = new EmbedAuthorBuilder()
                .WithName(Context.User.GlobalName)
                .WithIconUrl(Context.User.GetAvatarUrl())
                .WithUrl("https://yaflay.ru/");

            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithFooter(footer)
                .WithColor(5793266)
                .WithAuthor(author)
                .WithImageUrl(attachment?.Url)
                .Build();

            await DeferAsync(true);
            await FollowupAsync("Готово!", ephemeral: true);

            await Context.Channel.SendMessageAsync(embed: embed);

        }

        [SlashCommand("verification", "Отправляет сообщение верификации")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task sendVerificationEmbed()
        {
            await DeferAsync(true);
            var embed = new EmbedBuilder()
                .WithTitle("**Верификация игроков**")
                .WithDescription($"Если что-то случилось, и вам не выдается роль <@&1136564585420304444>, то нажмите на кнопку ниже!")
                .WithImageUrl("")
                .WithColor(Color.Blue)
                .Build();
            var components = new ComponentBuilder()
                .WithButton(
                new ButtonBuilder()
                    .WithLabel("Верификация")
                    .WithCustomId("UserVerification")
                    .WithStyle(ButtonStyle.Success)
                )
                .Build();
            await Context.Channel.SendMessageAsync(embed: embed, components: components);
            await FollowupAsync("Ok", ephemeral: true);
        }
        [SlashCommand("раздача-зарплаты", "Берет данные из баз данных и раздает кому надо")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task giveAvanse()
        {
            await DeferAsync(true);
            int allCount = 0;
            var allReports = Startup.appDbContext.Reports.ToArray();
            var allEmployee = new Dictionary<string, int>();
            

            foreach (var report in allReports)
            {
                if (allEmployee.TryGetValue(report.Employee, out _))
                {
                    allEmployee[report.Employee] += (int)report.type;
                }
                else
                {
                    allEmployee.Add(report.Employee, (int)report.type);
                }

            }
            Startup startup = new();
            foreach (var employee in allEmployee)
            {
                try
                {

                    var userData = await startup.getUserData(employee.Key);
                    await Startup.sp.CreateTransaction(userData.cardsOwned.First().number, employee.Value, $"zp {employee.Key}");
                    await Console.Out.WriteLineAsync($"{employee.Key}, {employee.Value}");
                    allCount += employee.Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}, {employee.Key}");
                }
            }
            await FollowupAsync($"Готово! Раздал {allCount} АР ||{allEmployee.ToArray()}||", ephemeral: true);
        }
    }
}
