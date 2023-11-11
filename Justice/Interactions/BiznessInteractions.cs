using Discord;
using Discord.Interactions;
using DiscordApp.Database.Tables;
using DiscordApp.Justice.Modals;
using spworlds.Types;

namespace DiscordApp.Justice.Interactions
{
    public class BiznessInteraction : InteractionModuleBase<SocketInteractionContext>
    {
        [ComponentInteraction("NewIndividualEntrepreneur")]
        public async Task AplyWork()
            => await Context.Interaction.RespondWithModalAsync<INewIndividualEntrepreneur>("newIndividualEterpreneurModal");
        [ComponentInteraction("NewBizness")]
        public async Task reCreatePassport()
            => await Context.Interaction.RespondWithModalAsync<INewBizness>("NewBiznessModal");

        [ModalInteraction("newIndividualEterpreneurModal")]
        public async Task newIndividualEterpreneur(INewIndividualEntrepreneur modal)
        {

            Passport? applicant = await Startup.appDbContext.Passport.FindAsync(int.Parse(modal.passportId));
            if (applicant == null)
            {
                await FollowupAsync("Ошибка! Такого паспорта не существует. Попробуйте старого бота.", ephemeral: true);
                return;
            }
            User spApplicant = await User.CreateUser(applicant.Applicant);
            var employees = new List<int>
            {
                applicant.Id
            };

            Bizness biznessDB = new()
            {
                Applicant = applicant,
                Employee = ((IGuildUser)Context.User).DisplayName,
                BiznessEmployes = employees.ToArray(),
                BiznessName = modal.Name,
                BiznessType = modal.BiznessType,
                CardNumber = modal.CardNumber,
                Date = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
            Reports report = new()
            {
                Employee = ((IGuildUser)Context.User).DisplayName,
                type = Types.ReportTypes.Bizness
            };
            await Startup.appDbContext.Reports.AddAsync(report);
            await Startup.appDbContext.Bizness.AddAsync(biznessDB);

            if (!modal.Name.StartsWith("test")) { await Startup.appDbContext.SaveChangesAsync(); }

            var fieldBuilder = new EmbedFieldBuilder()
                .WithName("Данные:")
                .WithValue($"```Аппликант: {applicant.Applicant}\nНазвание: {modal.Name}\nТип деятельности: {modal.BiznessType}\nНомер карты:{modal.CardNumber}```");
            var author = new EmbedAuthorBuilder()
                .WithIconUrl(Context.User.GetAvatarUrl())
                .WithName(((IGuildUser)Context.User).DisplayName);
            var embed = new EmbedBuilder()
                .WithTitle("Новый ИП зарегестрирован!")
                .WithAuthor(author)
                .WithFields(fieldBuilder)
                .WithThumbnailUrl(spApplicant.GetSkinPart(SkinPart.face))
                .WithColor(Color.Blue)
                .Build();
            await FollowupAsync("Готово!", ephemeral: true);
            var channel = Context.Guild.GetChannel(1108006685626355733) as ITextChannel;

            await channel.SendMessageAsync(embed: embed);
        }

        [ModalInteraction("NewBiznessModal")]
        public async Task newBizness(INewBizness modal)
        {

            Passport? applicant = await Startup.appDbContext.Passport.FindAsync(int.Parse(modal.passportId));
            
            if (applicant == null)
            {
                await FollowupAsync("Ошибка! Такого паспорта не существует. Попробуйте старого бота.", ephemeral: true);
                return;
            }
            User spApplicant = await User.CreateUser(applicant.Applicant);
            var employees = new List<int>
            {
                applicant.Id
            };
            string employeesNames = "";
            foreach (var passportId in modal.BiznessEmployee.Split(","))
            {
                Passport? employee = await Startup.appDbContext.Passport.FindAsync(int.Parse(passportId));
                if (employee != null) { employees.Add(employee.Id); employeesNames += $" {employee.Applicant}"; }
                else 
                {
                    await FollowupAsync($"У {passportId} указан неправильный номер паспорта.", ephemeral: true);
                    return;
                }
            }

            Bizness biznessDB = new()
            {
                Applicant = applicant,
                Employee = ((IGuildUser)Context.User).DisplayName,
                BiznessEmployes = employees.ToArray(),
                BiznessName = modal.Name,
                BiznessType = modal.BiznessType,
                CardNumber = modal.CardNumber,
                Date = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
            Reports report = new()
            {
                Employee = ((IGuildUser)Context.User).DisplayName,
                type = Types.ReportTypes.Bizness
            };
            await Startup.appDbContext.Reports.AddAsync(report);
            await Startup.appDbContext.Bizness.AddAsync(biznessDB);

            if (!modal.Name.StartsWith("test")) { await Startup.appDbContext.SaveChangesAsync(); }

            var fieldBuilder = new EmbedFieldBuilder()
                .WithName("Данные:")
                .WithValue($"```Аппликант: {applicant.Applicant}\nНазвание: {modal.Name}\nТип деятельности: {modal.BiznessType}\nНомер карты:{modal.CardNumber}\nСотрудники:{employeesNames}```");
            var author = new EmbedAuthorBuilder()
                .WithIconUrl(Context.User.GetAvatarUrl())
                .WithName(((IGuildUser)Context.User).DisplayName);
            var embed = new EmbedBuilder()
                .WithTitle("Новый ООО зарегестрирован!")
                .WithAuthor(author)
                .WithFields(fieldBuilder)
                .WithThumbnailUrl(spApplicant.GetSkinPart(SkinPart.face))
                .WithColor(Color.Blue)
                .Build();
            await FollowupAsync("Готово!", ephemeral: true);
            var channel = Context.Guild.GetChannel(1108006685626355733) as ITextChannel;

            await channel.SendMessageAsync(embed: embed);
        }

    }
}
