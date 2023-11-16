using Discord;
using Discord.Interactions;

namespace DiscordApp.Justice.Modals
{
    public class ICitiesModal : IModal
    {
        public string Title => "Регистрация базы";

        [InputLabel("Название на карте")]
        [ModalTextInput("Name", TextInputStyle.Short, placeholder: "Сигмасити", maxLength: 100)]
        public string Name { get; set; }

        [InputLabel("Описание на карте")]
        [ModalTextInput("BaseDescription", TextInputStyle.Paragraph, placeholder: "Меня зовут Кира Йошикаге...")]
        public string description { get; set; }

        [InputLabel("Х координата")]
        [ModalTextInput("xCoordinate", TextInputStyle.Short, placeholder: "70", maxLength: 4)]
        public int xCoordinate { get; set; }

        [InputLabel("Y координата")]
        [ModalTextInput("yCoordinate", TextInputStyle.Short, placeholder: "90", maxLength: 4)]
        public int yCoordinate { get; set; }

    }
}
