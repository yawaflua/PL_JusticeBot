using Discord;
using Discord.Interactions;

namespace DiscordApp.Justice.Modals
{
    public class INewArtModal : IModal
    {
        public string Title => "Новый патент на арт";

        [InputLabel("ID паспорта")]
        [ModalTextInput("passportId", TextInputStyle.Short, placeholder: "97664", maxLength: 10)]
        public int PassportId { get; set; }

        [InputLabel("Название арта")]
        [ModalTextInput("artName", TextInputStyle.Short, placeholder: "Пикачу", maxLength: 100)]
        public string Name { get; set; }

        [InputLabel("Номера карт арта(Через запятую)")]
        [ModalTextInput("mapNumbers", TextInputStyle.Short, placeholder: "14322, 14323")]
        public string MapNumbers { get; set; }

        [InputLabel("Размер арта")]
        [ModalTextInput("artSize", TextInputStyle.Short, placeholder: "3х1")]
        public string Size { get; set; }
        [InputLabel("Разрешено ли перепродавать(0/1)")]
        [ModalTextInput("artResell", TextInputStyle.Short, placeholder: "0 - нет, 1 - да")]
        public int IsAllowedToResell { get; set; }
    }

    public class INewBookModal : IModal
    {
        public string Title => "Новый патент на книгу";

        [InputLabel("ID паспорта")]
        [ModalTextInput("passportId", TextInputStyle.Short, placeholder: "97664", maxLength: 10)]
        public int PassportId { get; set; }

        [InputLabel("Название книги")]
        [ModalTextInput("bookName", TextInputStyle.Short, placeholder: "Пикачу", maxLength: 100)]
        public string Name { get; set; }

        [InputLabel("Аннотация")]
        [ModalTextInput("bookAnnotation", TextInputStyle.Short, placeholder: "14322, 14323")]
        public string Annotation { get; set; }

        [InputLabel("Жанр")]
        [ModalTextInput("bookJanre", TextInputStyle.Short, placeholder: "3х1")]
        public string Janre { get; set; }

        [InputLabel("Разрешено ли перепродавать(0/1)")]
        [ModalTextInput("bookResell", TextInputStyle.Short, placeholder: "0 - нет, 1 - да")]
        public int IsAllowedToResell { get; set; }
    }
}
