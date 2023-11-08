using Discord;
using Discord.Interactions;

namespace DiscordApp.Justice.Modals
{
    public class IPassportGetter : IModal
    {
        public string Title => "Поиск паспорта";
        [InputLabel("Данные паспорта")]
        [ModalTextInput("passport", TextInputStyle.Short, placeholder: "yawaflua или 97652", maxLength: 100)]
        public string passport { get; set; }

    }
}
