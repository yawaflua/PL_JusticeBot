using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp;
using DiscordApp.Auth;
using DiscordApp.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using spworlds;
using System;

namespace DiscordApp
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public static IServiceProvider serviceProvider;
        public static AppDbContext appDbContext;
        public static SPWorlds sp;
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
            string CardId = "9bfb91d2-8e14-4c6d-b91d-8a55ab4c6559";
            string CardToken = "L3NsEQsW69sM3Gm0v/+hHDaU4TFocp7F";
            sp = new SPWorlds(CardId, CardToken);
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
                .AddDbContext<AppDbContext>(c => c.UseNpgsql(@"Host=185.104.112.180;Username=yaflay;Password=hQgtruasSS;Database=spw_store"))
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