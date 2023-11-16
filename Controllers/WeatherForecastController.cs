using Discord;
using DiscordApp.Database;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace DiscordApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class redirects : ControllerBase
    {

        [HttpGet("/redirects/{uri}")]
        public IActionResult Get(string uri, [FromBody] string? bodyContent)
        {
            var data = Startup.appDbContext.Redirects.First(k => k.Id == uri);
            if (data.RedirectType == Types.RedirectType.None)
            {
                data.RedirectType = Types.RedirectType.Redirected;
                Startup.appDbContext.Redirects.Update(data);
                Startup.appDbContext.SaveChanges();
                return Redirect(data.url);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost("/redirects/{uri}")]
        public IActionResult Post(string uri, [FromBody] string bodyContent) 
        {
            JsonNode jsonBodyContent = JsonNode.Parse(bodyContent);
            string[] paymentData = jsonBodyContent["data"].ToString().Split(";");
            var channelId = paymentData[1].Split(":")[1];
            var channel = Startup.discordSocketClient.GetChannel(ulong.Parse(channelId)) as ITextChannel;
            var message = channel.GetMessagesAsync().LastAsync().Result.Last() as IUserMessage;
            message.ModifyAsync(func =>
            {
                func.Content = "Успешно оплачено!";
                func.Components = new ComponentBuilder()
                    .WithButton("Создание заявки", "addBaseOnMapModalSender")
                    .Build();
            }).RunSynchronously();

            return Redirect(message.GetJumpUrl());
        }

    }
}