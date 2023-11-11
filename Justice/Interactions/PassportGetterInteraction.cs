using Discord;
using Discord.Interactions;
using DiscordApp.Database.Tables;
using DiscordApp.Justice.Modals;

namespace DiscordApp.Justice.Interactions
{
    public class PassportGetterInteraction : InteractionModuleBase<SocketInteractionContext>
    {
        [ComponentInteraction("searchPassport")]
        public async Task searchPassportInteraction() => await RespondWithModalAsync<IPassportGetter>("GetPassportModal");

        [ModalInteraction("GetPassportModal")]
        public async Task getPassportInteraction(IPassportGetter modal)
        {
            await DeferAsync(true);
            int passportId;
            string thumbnailUrl;
            Passport passport;
            bool isInteger = int.TryParse(modal.passport, out passportId);
            if (isInteger)
            {
                passport = await Startup.appDbContext.Passport.FindAsync(passportId);
            }
            else
            {
                passport = Startup.appDbContext.Passport.Where(k => k.Applicant == modal.passport).FirstOrDefault();
            }

            if (passport == null)
            {
                await FollowupAsync("Игрок или паспорт не найден в базе данных, попробуйте использовать старого бота.", ephemeral: true);
                return;
            }
            var fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder().WithName("Никнейм").WithValue(passport.Applicant).WithIsInline(true),
                new EmbedFieldBuilder().WithName("РП Имя").WithValue(passport.RpName).WithIsInline(true),
                new EmbedFieldBuilder().WithName("Гендер").WithValue(passport.Gender).WithIsInline(false),
                new EmbedFieldBuilder().WithName("Благотворитель").WithValue((int)passport.Support).WithIsInline(false),
                new EmbedFieldBuilder().WithName("Дата рождения").WithValue($"<t:{passport.birthDate}:D>").WithIsInline(false),
                new EmbedFieldBuilder().WithName("Номер паспорта").WithValue(passport.Id).WithIsInline(true),
                new EmbedFieldBuilder().WithName("Годен до").WithValue($"<t:{passport.Date}:D>").WithIsInline(true),
                new EmbedFieldBuilder().WithName("Паспортист").WithValue($"<@{passport.Employee}>").WithIsInline(true)
            };
            try
            {
                thumbnailUrl = spworlds.Types.User.CreateUser(passport.Applicant).Result.GetSkinPart(spworlds.Types.SkinPart.face);
            }
            catch
            {
                thumbnailUrl = null;
            }
             var embed = new EmbedBuilder()
                .WithTitle("**Информация о паспорте**")
                .WithFields(fields)
                .WithThumbnailUrl(thumbnailUrl)
                .Build();
            await FollowupAsync(embed:embed, ephemeral: true);
                
        }
    }
}
