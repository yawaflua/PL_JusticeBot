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
                var guild = Startup.discordSocketClient.GetGuild(1107742957458685985);
                var channel = guild.GetChannel(channelid) as ITextChannel;
                var message = channel.GetMessagesAsync().LastOrDefaultAsync().Result.FirstOrDefault() as IUserMessage;
                await message.ModifyAsync(func =>
                {
                    func.Content = "������� ��������!";
                    func.Components = new ComponentBuilder()
                        .WithButton("�������� ������", "addBaseOnMapModalSender")
                        .Build();
                });

                return Redirect(message.GetJumpUrl());
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