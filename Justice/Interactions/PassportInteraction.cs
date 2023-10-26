using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using DiscordApp.Database.Tables;
using DiscordApp.Enums;
using Microsoft.EntityFrameworkCore;
using spworlds.Types;
using System.Globalization;
using System.IO;
using System.Xml;

namespace DiscordApp.Justice.Interactions
{
    public class PassportInteraction : InteractionModuleBase<SocketInteractionContext>
    {

        [ComponentInteraction("newPassport")]
        public async Task AplyWork()
            => await Context.Interaction.RespondWithModalAsync<NewPassportModal>("passportModal");

        [ModalInteraction("passportModal")]
        public async Task createPassportInteraction(NewPassportModal modal)
        {
            await DeferAsync(true);
            string name = modal.NickName;
            string RpName = modal.RPName;
            int supporterInt = modal.Supporter;
            string birthday = modal.Birthday;
            string gender = modal.Gender;

            SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
            Supporter supporter;
            Random random = new();
            spworlds.Types.User spUser = await spworlds.Types.User.CreateUser(name);

            DateTimeOffset toTime = DateTime.Now.AddDays(14);
            DateTime birthDate;
            int id = random.Next(00001, 99999);
            long unixTime;

            try
            {
                birthDate = DateTime.Parse(birthday);
                unixTime = ((DateTimeOffset)birthDate).ToUnixTimeSeconds();
                if (birthDate.AddDays(14) < DateTime.Now)
                {
                    await FollowupAsync($"Возможно, игрок {name} больше не новичек, и бесплатный паспорт ему не положен!", ephemeral: true);
                }
            }
            catch
            {
                await FollowupAsync($"Возможно, с датой {modal.Birthday} какая-то ошибка, попробуйте такой тип: 14.02.2023", ephemeral: true);
                return;
            }

            switch (supporterInt)
            {
                case 0:
                    supporter = Supporter.None;
                    break;
                case 1:
                    supporter = Supporter.FirstLvl;
                    break;
                case 2:
                    supporter = Supporter.SecondLvl;
                    break;
                case 3:
                    supporter = Supporter.ThirdLvl;
                    break;

                default:
                    await FollowupAsync("Неправильно указан уровень благотворителя. Используйте числа от 0 до 3(в зависимости от уровня)", ephemeral: true);
                    return;
            }

            var passportData = new EmbedFieldBuilder()
                .WithName("Данные паспорта:")
                .WithValue($"```\nИмя: {name}\nРП Имя: {RpName}\nАйди: {id}\nБлаготворитель: {supporter}\nГендер: {gender}\nДата рождения: {birthDate.ToShortDateString()}```")
                .WithIsInline(true);

            var author = new EmbedAuthorBuilder()
                .WithName(user.DisplayName)
                .WithIconUrl(user.GetDisplayAvatarUrl());

            var faceUrl = "https://visage.surgeplay.com/face/64/" + spUser.Uuid;
            Console.WriteLine(faceUrl);

            var embed = new EmbedBuilder()
                .WithTitle("**Новый паспорт**")
                .AddField(passportData)
                .AddField(new EmbedFieldBuilder().WithName("Составитель: ").WithValue(user.GlobalName).WithIsInline(true))
                .AddField(new EmbedFieldBuilder().WithName("Доступен до: ").WithValue($"<t:{toTime.ToUnixTimeSeconds()}:D>").WithIsInline(true))
                .WithThumbnailUrl(faceUrl)
                .WithColor(Color.DarkBlue)
                .WithAuthor(author)
                .WithTimestamp(toTime)
                .Build();

            Passport passport = new()
            {
                Employee = user.Id,
                RpName = RpName,
                Gender = gender,
                Date = unixTime,
                Applicant = name,
                Id = id,
                Support = supporter
            };

            if (Startup.appDbContext.Passport.FindAsync(passport.Id).Result != null)
            {
                bool isUnical = false;
                while (!isUnical)
                {

                    id = random.Next(00001, 99999);
                    passport.Id = id;
                    Console.WriteLine(passport.Id);
                    if (Startup.appDbContext.Passport.FindAsync(passport.Id).Result == null) { break; }
                }
            }




            await Startup.appDbContext.Passport.AddAsync(passport);
            await Startup.appDbContext.SaveChangesAsync();
            await FollowupAsync($"ID для паспорта: {id}", embed: embed, ephemeral: true);

            var channel = Context.Guild.GetChannel(1108006685626355733) as ITextChannel;

            var message = await channel.SendMessageAsync(embed: embed);
        }
    }

    public class NewPassportModal : IModal
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

}
