using Discord;
using Discord.Interactions;

namespace DiscordApp.Justice.Modals
{
    public class INotaryModal : IModal
    {
        public string Title => "Сертификация док-ов";
        [InputLabel("ID паспорта")]
        [ModalTextInput("passportId", TextInputStyle.Short, placeholder: "96534", maxLength: 10)]
        public string passportId { get; set; }

        [InputLabel("Тип документа")]
        [ModalTextInput("documentType", TextInputStyle.Short, placeholder: "Пользовательское соглашение", maxLength: 100)]
        public string documentType { get; set; }

        [InputLabel("Текст из документа")]
        [ModalTextInput("textFromDocument", TextInputStyle.Paragraph)]
        public string documentText { get; set; }

    }
}
