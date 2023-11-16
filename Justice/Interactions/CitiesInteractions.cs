using Discord;
using Discord.Interactions;
using DiscordApp.Database.Tables;
using DiscordApp.Justice.Modals;
using DiscordApp.Types;

namespace DiscordApp.Justice.Interactions
{
    public class CitiesInteractions : InteractionModuleBase<SocketInteractionContext>
    {
        [ComponentInteraction("addBaseOnMap")]
        public async Task addBaseInteraction()
        {
            await DeferAsync(true);
            var thread = await ((ITextChannel)Context.Channel).CreateThreadAsync($"{Context.User.GlobalName}-addCityOnMap", ThreadType.PrivateThread) as IThreadChannel;
            await thread.AddUserAsync(((IGuildUser)Context.User));
            await FollowupAsync(thread.Mention, ephemeral:true);
            var redirectUri = Guid.NewGuid().ToString();

            var paymentData = new spworlds.Types.PaymentData()
            {
                Amount = 16,
                Data = $"user:{Context.User.Id};channel:{thread.Id};",
                RedirectUrl = $"https://discord.yawaflua.ru/redirects/{redirectUri}&channelid={thread.Id}",
                WebHookUrl = $"https://discord.yawaflua.ru/redirects/{redirectUri}"
            };
            var uri = await Startup.sp.InitPayment(paymentData);
            var redirectTable = new Redirects() { Id = redirectUri , url = uri};
            Startup.appDbContext.Redirects.Add(redirectTable);
            Startup.appDbContext.SaveChanges();
            await thread.SendMessageAsync("Нажмите на кнопку ниже для оплаты", components: new ComponentBuilder().WithButton("Оплатить", url: $"https://discord.yawaflua.ru/redirects/{redirectUri}", style: ButtonStyle.Link).Build());

        }

        [ComponentInteraction("addBaseOnMapModalSender")]
        public async Task sendModal() => await RespondWithModalAsync<ICitiesModal>("newBaseOnMapModal");

        [ModalInteraction("newBaseOnMapModal")]
        public async Task addBaseOnMapModal(ICitiesModal modal)
        {
            await DeferAsync(true);
            var defaultSpUser = await Startup.sp.GetUser(Context.User.Id.ToString());
            var fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder()
                    .WithName("Название")
                    .WithValue(modal.Name),
                new EmbedFieldBuilder()
                    .WithName("Описание")
                    .WithValue($"{modal.description}"),
                new EmbedFieldBuilder()
                    .WithName("X Координата")
                    .WithValue(modal.xCoordinate),
                new EmbedFieldBuilder()
                    .WithName("Y Координата")
                    .WithValue(modal.yCoordinate),
            };
            var footer = new EmbedFooterBuilder()
                .WithText(Context.User.Id.ToString())
                .WithIconUrl(((IGuildUser)Context.User).GetDisplayAvatarUrl());
            var components = new ComponentBuilder()
                .WithButton(customId: "accessNewBase", label: "✅")
                .WithButton(customId: "declineNewBase", label: "❌")
                .Build();
            var embed = new EmbedBuilder()
                .WithTitle("Заявка на регистрацию базы")
                .WithThumbnailUrl(defaultSpUser.GetSkinPart(spworlds.Types.SkinPart.face))
                .WithFooter(footer)
                .WithFields(fields)
                .Build();
            var channel = Context.Guild.GetTextChannel(1174722397820174439);
            await channel.SendMessageAsync("@ #here", embed: embed, components: components);
            await FollowupAsync("Заявка подана и передана ответственным лицам. Ожидайте!", ephemeral: true);
            var threadChannel = Context.Channel as IThreadChannel;
            await threadChannel.ModifyAsync(k => k.Archived = true);
        }

        [ComponentInteraction("accessNewBase")]
        public async Task addBaseOnMap()
        {
            await DeferAsync(true);
            
            var embed = Context.Interaction.GetOriginalResponseAsync().Result.Embeds.First();
            var embedFields = embed.Fields;

            var startup = new Startup();
            var defaultSpUser = await Startup.sp.GetUser(embed.Footer.Value.Text);
            var spUser = await startup.getUserData(defaultSpUser.Name);

            int xCoordinate;
            int zCoordinate;

            if(spUser.city != null)
            {
                await FollowupAsync("Игрок уже состоит в каком-то городе!");
                return;
            }

            if (!int.TryParse(embedFields[2].Value, out xCoordinate) || !int.TryParse(embedFields[3].Value, out zCoordinate))
            {
                await FollowupAsync("Ошибка, координаты неправильные!");
                return;
            }

            var city = new SPCity()
            {
                description = embedFields[1].Value,
                name = embedFields[0].Value,
                mayor = spUser.id,
                x = xCoordinate,
                z = zCoordinate,
            };

            var components = new ComponentBuilder()
                .WithButton(customId: "accessNewBase", label: "✅", disabled: true)
                .WithButton(customId: "declineNewBase", label: "❌", disabled: true)
                .Build();

            await ModifyOriginalResponseAsync(func => { func.Components = components; func.Content = $"Заявку принял {Context.User.Mention}"; });

            try
            {
                await startup.addSityOnMap(city);
                await FollowupAsync("Все готово!", ephemeral: true);
            }
            catch
            {
                await FollowupAsync("Какая-то ошибка, чекни логи", ephemeral: true);
            }

        }
        [ComponentInteraction("declineNewBase")]
        public async Task declineBaseOnMap()
        {
            await DeferAsync(true);

            var components = new ComponentBuilder()
                .WithButton(customId: "accessNewBase", label: "✅", disabled: true)
                .WithButton(customId: "declineNewBase", label: "❌", disabled: true)
                .Build();

            await ModifyOriginalResponseAsync(func => { func.Components = components; func.Content = $"Заявку отозвал {Context.User.Mention}"; });

            try
            {
                await FollowupAsync("Все готово!", ephemeral:true);
            }
            catch
            {
                await FollowupAsync("Какая-то ошибка, чекни логи", ephemeral:true);
            }

        }

    }
}
