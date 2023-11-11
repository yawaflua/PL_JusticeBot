using Discord;
using Discord.Interactions;
using DiscordApp.Database.Tables;
using DiscordApp.Justice.Modals;
using DiscordApp.Types;
using DiscordApp.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace DiscordApp.Justice.Interactions
{
    public class NotaryInteractions : InteractionModuleBase<SocketInteractionContext>
    {
        [ComponentInteraction("NewDocumentCertificate")]
        public async Task newCertificatedDocument()
            => await Context.Interaction.RespondWithModalAsync<INotaryModal>("newCertificatedDocument");

        [ModalInteraction("newCertificatedDocument")]
        public async Task newCertificatedDocumentModal(INotaryModal modal)
        {
            await DeferAsync(true);
            int passportId;
            int documentId;
            int certificateId;
            string thumbnailUrl;
            Passport passport;
            Random random = new Random();
            Utilities.Utilities.IdGenerator(out certificateId);
            documentId = Startup.appDbContext.Certificates.OrderBy(t => t.Id).FirstOrDefault().Id;
            documentId += 1;
            if (!int.TryParse(modal.passportId, out passportId))
            {
                await FollowupAsync($"Айди паспорта еще старое, попробуй использовать другого бота.", ephemeral: true);
                return;
            }
            else if (Utilities.Utilities.IsPassport(passportId, out passport))
            {
                await FollowupAsync($"Паспорт не найден в базе данных, попробуй написать правильно", ephemeral: true);
                return;
            }


            Certificate certificate = new()
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                DocumentId = documentId,
                Employee = ((IGuildUser)Context.User).DisplayName,
                Id = certificateId,
                passport = passport,
                Text = modal.documentText,
                DocumentType = modal.documentType
            };
            Reports report = new()
            {
                Employee = Startup.sp.GetUser(Context.User.Id.ToString()).Result.Name,
                type = ReportTypes.editPassport
            };
            await Startup.appDbContext.Reports.AddAsync(report);
            await Startup.appDbContext.Certificates.AddAsync(certificate);
            if (!modal.documentText.StartsWith("test"))
                await Startup.appDbContext.SaveChangesAsync();

            try
            {
                thumbnailUrl = spworlds.Types.User.CreateUser(passport.Applicant).Result.GetSkinPart(spworlds.Types.SkinPart.face);
            }
            catch
            {
                thumbnailUrl = null;
            }

            var author = new EmbedAuthorBuilder()
                .WithName(((IGuildUser)Context.User).DisplayName)
                .WithIconUrl(((IGuildUser)Context.User).GetDisplayAvatarUrl());

            var field1 = new EmbedFieldBuilder()
                .WithName("Данные документа:")
                .WithValue($"ID документа: {documentId}\nID паспорта: {passport.Id}\nID сертификата: {certificateId}\n\nАпликант: {passport.Applicant}");
            var field2 = new EmbedFieldBuilder()
                .WithName("Текст документа:")
                .WithValue($"```{modal.documentText}```");
            var embed = new EmbedBuilder()
                .WithTitle("Заверен новый документ")
                .WithAuthor(author)
                .WithThumbnailUrl(thumbnailUrl)
                .WithFields(field1)
                .WithFields(field2)
                .WithColor(Color.DarkBlue)
                .Build();
            var channel = Context.Guild.GetChannel(1172879659193606164) as ITextChannel;
            await channel.SendMessageAsync(embed: embed);
            await FollowupAsync(embed: embed);

        }
    }
}
