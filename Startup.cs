using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp;
using DiscordApp.Auth;
using DiscordApp.Controllers;
using DiscordApp.Database;
using DiscordApp.Types;
using DotNetEd.CoreAdmin;
using DotNetEd.CoreAdmin.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public static DiscordSocketClient discordSocketClient;
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
            string CardId = "";
            string CardToken = "";
            string token = "";
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
            string ok = @"{}";
            var content = new StringContent(ok);
            var responseMessage = client.PostAsync("auth/refresh_token", content).Result;
            Console.WriteLine(responseMessage.Content.ReadAsStringAsync().Result.ToString());
        }
        public async Task<SPUser> getUserData(string userName)
        {
            var request = await client.GetAsync($"pl/accounts/{userName}");
            await Console.Out.WriteLineAsync(request.Content.ReadAsStringAsync().Result);
            SPUser response = JsonConvert.DeserializeObject<SPUser>(request.Content.ReadAsStringAsync().Result.ToString());
            return response;
        }
        public async Task<IEnumerable<SPCity>> getAllSities()
        {
            var citiesArray = new List<SPCity>();
            var request = await client.GetAsync("https://spworlds.ru/api/pl/cities");
            JsonNode jsonBody = await request.Content.ReadFromJsonAsync<JsonNode>();
            foreach (JsonNode node in jsonBody.AsArray())
            {
                citiesArray.Add(JsonConvert.DeserializeObject<SPCity>(node.ToJsonString()));
            }
            return citiesArray;
        }

        public async Task<SPCity?> addSityOnMap(SPCity city)
        {
            try
            {
                var request = await client.PostAsync("https://spworlds.ru/api/pl/cities", JsonContent.Create(city));
                if (request.StatusCode.HasFlag(HttpStatusCode.OK))
                {
                    return city;
                }
                else
                {
                    throw new Exception($"Unknown error from site! {request.Content.ReadAsStringAsync().Result}") ;
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }

        }

        public async Task<SPCity> deleteSityFromMap(SPCity city)
        {
            var request = await client.DeleteAsync($"https://spworlds.ru/api/pl/cities/{city.id}");
            if (request.StatusCode.Equals(200))
            {
                return city;
            }
            else
            {
                throw new Exception("Unknown error from site!");
            }

        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCoreAdmin("yawaflua");
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
                .AddDbContext<AppDbContext>(c => c.UseNpgsql(@""))
                ;


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
            app.UseCors(k => { k.AllowAnyHeader(); k.AllowAnyMethod(); k.AllowAnyOrigin(); k.WithMethods("POST", "GET"); });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}