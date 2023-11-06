using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp;
using DiscordApp.Auth;
using DiscordApp.Database;
using DiscordApp.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using spworlds;
using System;
using System.Net;
using System.Text.Json.Nodes;

namespace DiscordApp
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public static IServiceProvider serviceProvider;
        public static AppDbContext appDbContext;
        public static SPWorlds sp;
        private readonly HttpClient client;
        private readonly DiscordSocketConfig socketConfig = new()
        {
            GatewayIntents = GatewayIntents.All,
            AlwaysDownloadUsers = true
        };

        public Startup()
        {
            configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "m.")
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            string CardId = "28fd1597-05a9-4ee0-8845-16ca37135081";
            string CardToken = "m+ziDmuTdFElD0vcKYnO3DS1h/9HuRGk";
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Ijk0NTMxNzgzMjI5MDMzNjc5OCIsInRva2VuVmVyc2lvbiI6MCwiaXAiOiIxODUuMTA0LjExMi4xODAiLCJpYXQiOjE2OTkxOTA5MzMsImV4cCI6MTcwMTc4MjkzM30.Z9ykVYIIsN0bF0BlNV6sgwdiRu-GNx-olmKRxl6OJHk";
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            client = new(handler);
            client.BaseAddress = new Uri("https://spworlds.ru/api/");
            cookieContainer.Add(client.BaseAddress, new Cookie("jeff", token));
            spwLogin();
            sp = new SPWorlds(CardId, CardToken);
        }

        public void spwLogin()
        {
            string ok = @"{
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
            var content = new StringContent(ok);
            var responseMessage = client.PostAsync("auth/refresh_token", content).Result;
            Console.WriteLine(responseMessage.Content.ReadAsStringAsync().Result.ToString());
        }
        public async Task<Root> getUserData(string userName)
        {
            var request = await client.GetAsync($"pl/accounts/{userName}");
            await Console.Out.WriteLineAsync(request.Content.ToString());
            Root response = JsonConvert.DeserializeObject<Root>(request.Content.ReadAsStringAsync().Result.ToString());
            return response;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services
                    //.AddHostedService<DiscordBotService>()
                    .AddHostedService<JusticeBotService>()
                    .AddSwaggerGen();


            services
                .AddSingleton(configuration)
                .AddSingleton(socketConfig)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<JusticeHandler>()
                .AddSingleton(sp)
                .AddDbContext<AppDbContext>(c => c.UseNpgsql(@"Host=185.104.112.180;Username=yaflay;Password=hQgtruasSS;Database=poopland"))
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Bearer";
                    options.DefaultChallengeScheme = "Bearer";
                }).AddScheme<AuthenticationSchemeOptions, AuthanticationByBearerToken>("Bearer", options => { });


            serviceProvider = services.BuildServiceProvider();
            appDbContext = serviceProvider.GetRequiredService<AppDbContext>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "/swagger/v1/swagger.json";
                });
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}