using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp;
using DiscordApp.Database.Tables;
using Microsoft.EntityFrameworkCore;
using spworlds;
using spworlds.Types;
using System.Runtime.InteropServices;

namespace DiscordApp.Justice.Events
{
    public class Events
    {
        public static async Task onJoinGuild(SocketGuildUser socketUser)
        {
            try
            {
                User SpwUser = await Startup.sp.GetUser(socketUser.Id.ToString());
                if (SpwUser.IsPlayer())
                {
                    IRole role;
                    if (socketUser.Guild.Id == 1165687128366268506) { role = socketUser.Guild.GetRole(1165687128366268511); }
                    else if (socketUser.Guild.Id == 1107742957458685985) { role = socketUser.Guild.GetRole(1136564585420304444); }
                    else { return; }
                    await socketUser.AddRoleAsync(role);
                    await socketUser.ModifyAsync(func =>
                    {
                        func.Nickname = SpwUser.Name;
                    });
                }
            }
            catch (Exception e) { await Console.Out.WriteLineAsync($"User {socketUser.DisplayName} not found as player!"); }
        }

        public static async Task onMessageCreate(SocketMessage message) 
        {
            /**
            var autoBranchesDatabase = await Startup.appDbContext.Autobranches.FindAsync(message.Channel.Id);
            var autoReactDatabase = await Startup.appDbContext.Autoreactions.ForEachAsync(s => s.ChannelId == message.Channel.Id);
            if (autoBranchesDatabase != null) 
            {
                await ((ITextChannel)message.Channel).CreateThreadAsync(autoBranchesDatabase.BranchName);
            }
            if (autoReactDatabase != false)
            {
                foreach (Autoreactions autoreaction in autoReactDatabase)
                {
                    var Emoji = Emote.Parse(autoreaction.EmoteId);
                    await message.AddReactionAsync(Emoji);
                }
            }
            **/
        }
    }
}
