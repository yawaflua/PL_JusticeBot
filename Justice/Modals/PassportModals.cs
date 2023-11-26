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

    }

    public class INewIdPassportModal : IModal
    {
        public string Title => "Обновление номера";

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

        [InputLabel("Новый номер паспорта")]
        [ModalTextInput("id", TextInputStyle.Short, maxLength: 5)]
        public string newId { get; set; }

    }

    public class IReNewPassportWithIdModal : IModal
    {
        public string Title => "Обновление номера";


        [InputLabel("Старый номер паспорта")]
        [ModalTextInput("oldid", TextInputStyle.Short, maxLength: 5)]
        public string Id { get; set; }

        [InputLabel("Новый номер паспорта")]
        [ModalTextInput("id", TextInputStyle.Short, maxLength: 5)]
        public string newId { get; set; }
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

