using Discord;
using Discord.Interactions;

namespace DiscordApp.Justice.Modals
{
    public class INewIndividualEntrepreneur : IModal
    {
        public string Title => "Регистрация ИП";
        [InputLabel("ID паспорта")]
        [ModalTextInput("nickname", TextInputStyle.Short, placeholder: "96534", maxLength: 10)]
        public string passportId { get; set; }

        [InputLabel("Название")]
        [ModalTextInput("Name", TextInputStyle.Short, placeholder: "ИП Оганесян", maxLength: 100)]
        public string Name { get; set; }

        [InputLabel("Тип деятельности")]
        [ModalTextInput("Type", TextInputStyle.Short, placeholder: "Продажа", maxLength: 200)]
        public string BiznessType { get; set; }

        [InputLabel("Номер карты")]
        [ModalTextInput("cardNumber", TextInputStyle.Short, placeholder: "70835", maxLength: 100)]
        public int CardNumber { get; set; }

    }
    public class INewBizness : IModal
    {
        public string Title => "Регистрация ИП";
        [InputLabel("ID паспорта")]
        [ModalTextInput("nickname", TextInputStyle.Short, placeholder: "96534", maxLength: 10)]
        public string passportId { get; set; }

        [InputLabel("Название")]
        [ModalTextInput("Name", TextInputStyle.Short, placeholder: "ИП Оганесян", maxLength: 100)]
        public string Name { get; set; }

        [InputLabel("Тип деятельности")]
        [ModalTextInput("Type", TextInputStyle.Short, placeholder: "Продажа", maxLength: 200)]
        public string BiznessType { get; set; }

        [InputLabel("Участники(через запятую)")]
        [ModalTextInput("Employee", TextInputStyle.Short, placeholder: "96534, 12742", maxLength: 200)]
        public string BiznessEmployee { get; set; }

        [InputLabel("Номер карты")]
        [ModalTextInput("cardNumber", TextInputStyle.Short, placeholder: "70835", maxLength: 100)]
        public int CardNumber { get; set; }
    }
}
