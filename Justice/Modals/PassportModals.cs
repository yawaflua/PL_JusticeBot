using Discord.Interactions;
using Discord;

namespace DiscordApp.Justice.Modals
{
    public class INewPassportModal : IModal
    {
        public string Title => "Создание паспорта";

        [InputLabel("Ник игрока")]
        [ModalTextInput("nickname", TextInputStyle.Short, placeholder: "YaFlay", maxLength: 90)]
        public string NickName { get; set; }

        [InputLabel("Благотворитель")]
        [ModalTextInput("Supporter", TextInputStyle.Short, placeholder: "1", maxLength: 5)]
        public int Supporter { get; set; }

        [InputLabel("РП Имя")]
        [ModalTextInput("rolePlayName", TextInputStyle.Short, placeholder: "Олег Бебров", maxLength: 200)]
        public string RPName { get; set; }

        [InputLabel("Пол")]
        [ModalTextInput("gender", TextInputStyle.Short, maxLength: 200)]
        public string Gender { get; set; }
        [InputLabel("Дата рождения")]
        [ModalTextInput("BirthDay", TextInputStyle.Short, placeholder: "16.02.2023", maxLength: 100)]
        public string Birthday { get; set; }

    }

    public class IReWorkPassportModal : IModal
    {
        public string Title => "Создание паспорта";

        [InputLabel("ID паспорта")]
        [ModalTextInput("id", TextInputStyle.Short, placeholder: "97166", maxLength: 7)]
        public string Id { get; set; }

        [InputLabel("Новые данные(0/1)")]
        [ModalTextInput("isNewPassportData", TextInputStyle.Short, placeholder: "1 - да, 0 - нет", maxLength: 1, initValue: "0")]
        public int IsNewPassport { get; set; }

    }

}

