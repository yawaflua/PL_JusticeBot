using Discord.Interactions;
using DiscordApp.Justice.Modals;
using DiscordApp.Database.Tables;
using Discord.WebSocket;
using Discord;
using spworlds.Types;

namespace DiscordApp.Justice.Interactions
{
    public class PatentInteraction : InteractionModuleBase<SocketInteractionContext>
    {
        [ComponentInteraction("artPatent")]
        public async Task artPatentInteractions() => await RespondWithModalAsync<INewArtModal>("newArtCallback");

        [ComponentInteraction("bookPatent")]
        public async Task bookPatentInteractions() => await RespondWithModalAsync<INewBookModal>("newBookCallback");

        [ModalInteraction("newArtCallback")]
        public async Task newArtModalInteraction(INewArtModal modal)
        {

            string name = modal.Name;
            string maps = modal.MapNumbers;
            string size = modal.Size;
            int passportId = modal.PassportId;
            bool isAllowedToReSell = modal.IsAllowedToResell == 1;

            Passport? passport = await Startup.appDbContext.Passport.FindAsync(passportId);
            if (passport == null) 
            {
                await FollowupAsync("ID паспорта не найден в базе данных. Попробуйте использовать старого бота.");
                return;
            }

            var mapDictionary = new List<int>();
            User spUser = await User.CreateUser(passport.Applicant);
            try
            {
                foreach (var map in maps.Split(','))
                {
                    mapDictionary.Add(int.Parse(map));
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"new error in patentInteractions 32-37 {ex.Message}");
                await FollowupAsync("Возникла ошибка при парсинге ID карт. Вы точно указали через запятую данные?");
                return;
            }
            ArtsPatents artsPatent = new()
            {
                Name = name,
                Employee = ((IGuildUser)Context.User).DisplayName,
                Size = size,
                Date = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Number = mapDictionary.ToArray(),
                isAllowedToResell = isAllowedToReSell,
                passport = passport
            };
            Reports report = new()
            {
                Employee = ((IGuildUser)Context.User).DisplayName,
                type = Types.ReportTypes.Patent
            };
            await Startup.appDbContext.Reports.AddAsync(report);
            await Startup.appDbContext.ArtPatents.AddAsync(artsPatent);

            if (!name.StartsWith("test")) { await Startup.appDbContext.SaveChangesAsync(); }

            var field = new EmbedFieldBuilder()
                .WithName("Данные патента")
                .WithValue($"```Название арта: {name} \nРазмер: {size} \nНомера: {maps} \nРазрешена перепродажа?: {isAllowedToReSell} \nАппликант: {passport.Applicant}```")
                .WithIsInline(false);
            var author = new EmbedAuthorBuilder()
                .WithIconUrl(Context.User.GetAvatarUrl())
                .WithName(((IGuildUser)Context.User).DisplayName);
            var Embed = new EmbedBuilder()
                .WithTitle("Новый патент!")
                .WithFields(field)
                .WithAuthor(author)
                .WithColor(Color.Blue)
                .WithCurrentTimestamp()
                .WithThumbnailUrl(spUser.GetSkinPart(SkinPart.face))
                .Build();
            await FollowupAsync("Готово!", ephemeral: true);
            var channel = Context.Guild.GetChannel(1108006685626355733) as ITextChannel;
            await channel.SendMessageAsync(embed: Embed);
        }
        [ModalInteraction("newBookCallback")]
        public async Task newBookModalInteraction(INewBookModal modal)
        {

            string name = modal.Name;
            string janre = modal.Janre;
            string annotation = modal.Annotation;
            int passportId = modal.PassportId;
            bool isAllowedToReSell = modal.IsAllowedToResell == 1;

            Passport? passport = await Startup.appDbContext.Passport.FindAsync(passportId);
            if (passport == null)
            {
                await FollowupAsync("ID паспорта не найден в базе данных. Попробуйте использовать старого бота.");
                return;
            }

            User spUser = await User.CreateUser(passport.Applicant);
            BooksPatents bookPatent = new()
            {
                Name = name,
                Employee = ((IGuildUser)Context.User).DisplayName,
                Janre = janre,
                Date = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Annotation = annotation,
                isAllowedToResell = isAllowedToReSell,
                passport = passport
            };
            Reports report = new()
            {
                Employee = ((IGuildUser)Context.User).DisplayName,
                type = Types.ReportTypes.Patent
            };
            await Startup.appDbContext.Reports.AddAsync(report);
            await Startup.appDbContext.BookPatents.AddAsync(bookPatent);

            if (!name.StartsWith("test")) { await Startup.appDbContext.SaveChangesAsync(); }

            var field = new EmbedFieldBuilder()
                .WithName("Данные патента")
                .WithValue($"```Название книги: {name} \nАннотация: {annotation} \nЖанр: {janre} \nРазрешена перепродажа?:{isAllowedToReSell} \nАппликант:{passport.Applicant}```")
                .WithIsInline(false);
            var author = new EmbedAuthorBuilder()
                .WithIconUrl(Context.User.GetAvatarUrl())
                .WithName(((IGuildUser)Context.User).DisplayName);
            var Embed = new EmbedBuilder()
                .WithTitle("Новый патент!")
                .WithFields(field)
                .WithAuthor(author)
                .WithColor(Color.Blue)
                .WithCurrentTimestamp()
                .WithThumbnailUrl(spUser.GetSkinPart(SkinPart.face))
                .Build();
            await FollowupAsync("Готово!", ephemeral: true);
            var channel = Context.Guild.GetChannel(1108006685626355733) as ITextChannel;
            await channel.SendMessageAsync(embed: Embed);
        }
    }
}
