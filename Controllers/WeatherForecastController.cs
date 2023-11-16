using Discord;
using DiscordApp.Database;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json.Nodes;

namespace DiscordApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class redirects : ControllerBase
    {

        [HttpGet("/redirects/{uri}&channelid={channelid}")]
        public async Task<IActionResult> Get(string uri, [FromBody] string? bodyContent, ulong channelid)
        {
            var data = Startup.appDbContext.Redirects.First(k => k.Id == uri);
            if (data.RedirectType == Types.RedirectType.None)
            {
                data.RedirectType = Types.RedirectType.Redirected;
                Startup.appDbContext.Redirects.Update(data);
                Startup.appDbContext.SaveChanges();
                return Redirect(data.url);
            }else if (data.RedirectType == Types.RedirectType.Redirected)
            {
                try
                {
                    var guild = Startup.discordSocketClient.GetGuild(1107742957458685985);
                    await Console.Out.WriteLineAsync(guild.Name);
                    var channel = guild.GetChannel(channelid) as ITextChannel;
                    await Console.Out.WriteLineAsync(channel.Name);
                    var message = channel.GetMessagesAsync().FirstOrDefaultAsync(k => k.Any(l => l.Author.Id == 1166079976446103602)).Result.FirstOrDefault() as IUserMessage;
                    await Console.Out.WriteLineAsync(message.Author.GlobalName);
                    await message.ModifyAsync(func =>
                    {
                        func.Content = "Successfully paid!";
                        func.Components = new ComponentBuilder()
                            .WithButton("Create request", "addBaseOnMapModalSender")
                            .Build();
                    });

                    return Redirect(message.GetJumpUrl());
                }catch(Exception ex)
                {
                    return Ok($"500: Error in discord client. Text error: {ex.Message}");
                }
            }   
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("/redirects/{uri}")]
        public IActionResult Post(string uri)
        {
            var data = Startup.appDbContext.Redirects.First(k => k.Id == uri);
            
            data.RedirectType = Types.RedirectType.Redirected;
            Startup.appDbContext.Redirects.Update(data);
            Startup.appDbContext.SaveChanges();
            return Redirect(data.url);


        }

    }
}