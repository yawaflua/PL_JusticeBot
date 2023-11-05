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
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Ijk0NTMxNzgzMjI5MDMzNjc5OCIsInRva2VuVmVyc2lvbiI6MCwiaXAiOiIxODUuMTA0LjExMi4xODAiLCJpYXQiOjE2OTkxOTA5MzMsImV4cCI6MTcwMTc4MjkzM30.Z9ykVYIIsN0bF0BlNV6sgwdiRu-GNx-olmKRxl6OJHk";
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient client = new(handler);
            client.BaseAddress = new Uri("https://spworlds.ru/api/");

            cookieContainer.Add(client.BaseAddress, new Cookie("jeff", token));
            string ok =  @"{
    ""id"": ""945317832290336798"",
    ""isAdmin"": false,
    ""minecraftUUID"": ""775f00d30da34275967d58cb50838b9f"",
    ""accounts"": [
        {
                ""id"": ""095ee127-578b-479e-90af-21b679546e09"",
            ""serverId"": ""spb"",
            ""roles"": [],
            ""isBanned"": false,
            ""banReason"": null,
            ""server"": {
                    ""id"": ""spb"",
                ""name"": ""СПб"",
                ""icon"": 1,
                ""hasSite"": true,
                ""economy"": {
                        ""campaignPrice"": 32,
                    ""pinPrice"": 0,
                    ""adPrice"": 32
                }
                }
            },
        {
                ""id"": ""96a89c09-63a1-44e9-9e10-abbf9f78483c"",
            ""serverId"": ""pl"",
            ""roles"": [
                ""mapmaker""
            ],
            ""isBanned"": false,
            ""banReason"": null,
            ""server"": {
                    ""id"": ""pl"",
                ""name"": ""PoopLand"",
                ""icon"": 4,
                ""hasSite"": true,
                ""economy"": {
                        ""campaignPrice"": 32,
                    ""pinPrice"": 0,
                    ""adPrice"": 32
                }
                }
            }
    ],
    ""bedrockUsername"": ""YaFlay"",
    ""username"": ""YaFlay"",
    ""hasTOTP"": false
}";
        var content = new StringContent(ok) ;
        await client.PostAsync("auth/refresh_token", content);
            

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
            foreach (var employee in allEmployee)
            {
                try
                {
                    var request = await client.GetAsync($"pl/accounts/{employee.Key}");
                    Console.WriteLine(request.Content.ReadAsStringAsync().Result);
                    JsonNode response = await request.Content.ReadFromJsonAsync<JsonNode>();

                    await Startup.sp.CreateTransaction(response["cardsOwned"][0]["number"].ToString(), employee.Value, $"zp {employee.Key}");
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
