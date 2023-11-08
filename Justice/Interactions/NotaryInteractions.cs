using Discord.Interactions;
using DiscordApp.Justice.Modals;
using DiscordApp.Utilities;

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
            Random random = new Random();
            IdChecker.IdLenghtIsLower(out certificateId);
            documentId = Startup.appDbContext.Certificates.OrderBy(t => t.Id).First().Id + 1;
            bool isInt = int.TryParse(modal.passportId, out passportId);
            if (!isInt)
            {
                await FollowupAsync($"Айди паспорта еще старое, попробуй использовать другого бота.", ephemeral: true);
            }
            else if (Startup.appDbContext.Passport.Find(passportId) == null)
            {
                await FollowupAsync($"Паспорт не найден в базе данных, попробуй написать правильно", ephemeral: true);
            }

        }
    }
}
