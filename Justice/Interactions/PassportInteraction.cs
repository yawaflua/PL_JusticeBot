using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp.Database.Tables;
using DiscordApp.Enums;
using spworlds.Types;
using DiscordApp.Justice.Modals;
using System;

namespace DiscordApp.Justice.Interactions
{
    public class PassportInteraction : InteractionModuleBase<SocketInteractionContext>
    {

        protected private int _AddDays = 60;


        [ComponentInteraction("newPassport")]
        public async Task AplyWork()
            => await Context.Interaction.RespondWithModalAsync<INewPassportModal>("passportModal");
        [ComponentInteraction("reworkPassport")]
        public async Task reCreatePassport()
            => await Context.Interaction.RespondWithModalAsync<IReWorkPassportModal>("reworkpassportModal");
        [ComponentInteraction("reNewPassportButton")]
        public async Task reNewPassportModal() => await Context.Interaction.RespondWithModalAsync<INewPassportModal>("ReNewPassportModal");


        [ModalInteraction("reworkpassportModal")]
        public async Task reCreatePassportInteraction(IReWorkPassportModal modal)
        {
            await DeferAsync(true);
            double passportId = modal.Id;
            bool recreatePassport = modal.IsNewPassport == 1;
            if (recreatePassport)
            {
                await FollowupAsync("Нажмите на кнопку ниже", components: new ComponentBuilder().WithButton(new ButtonBuilder("Кнопочка", "reNewPassportButton")).Build(), ephemeral: true);
            }
            else
            { 
                var passport = Startup.appDbContext.Passport.Where(x => x.Id == passportId).FirstOrDefault();
                if (passport == null) { await FollowupAsync("ID паспорта не правильный, или не существует.", ephemeral: true); return; }

                SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
                Random random = new();
                User spUser = await User.CreateUser(passport.Applicant);
                DateTimeOffset toTime;
                if (DateTimeOffset.FromUnixTimeSeconds(passport.birthDate).AddDays(14) < DateTimeOffset.Now)
                {
                    toTime = DateTime.Now.AddDays(_AddDays);
                }
                else
                {
                    toTime = DateTime.Now.AddDays(14);
                }

                int id = random.Next(00001, 99999);
                while (id.ToString().Length < 5) { id = random.Next(00001, 99999); }
                long unixTime = toTime.ToUnixTimeSeconds();
                long nowUnixTime = DateTimeOffset.Now.ToUnixTimeSeconds();

                passport.Id = id;
                passport.Date = nowUnixTime;

                var passportData = new EmbedFieldBuilder()
                .WithName("Данные паспорта:")
                .WithValue(@$"
Имя: {passport.Applicant}
РП Имя: {passport.RpName}
Айди: {id}
Благотворитель: {passport.Support}
Гендер: {passport.Gender}
Дата рождения: <t:{passport.birthDate}:D>")
                .WithIsInline(true);

                var author = new EmbedAuthorBuilder()
                    .WithName(user.DisplayName)
                    .WithIconUrl(user.GetDisplayAvatarUrl());

                var embed = new EmbedBuilder()
                    .WithTitle("**Паспорт переделан**")
                    .AddField(passportData)
                    .AddField(new EmbedFieldBuilder().WithName("Составитель: ").WithValue(user.GlobalName).WithIsInline(true))
                    .AddField(new EmbedFieldBuilder().WithName("Доступен до: ").WithValue($"<t:{toTime.ToUnixTimeSeconds()}:D>").WithIsInline(true))
                    .WithThumbnailUrl(spUser.GetSkinPart(SkinPart.face))
                    .WithColor(Color.DarkBlue)
                    .WithAuthor(author)
                    .WithTimestamp(toTime)
                    .Build();

                if (Startup.appDbContext.Passport.FindAsync(passport.Id).Result != null)
                {
                    bool isUnical = false;
                    while (!isUnical)
                    {
                        id = random.Next(00001, 99999);
                        passport.Id = id;
                        while (id.ToString().Length < 5) { id = random.Next(00001, 99999); }
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
        [ModalInteraction("ReNewPassportModal")]
        public async Task renewPassportInteraction(INewPassportModal modal)
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
            User spUser = await User.CreateUser(name);

            DateTimeOffset toTime;
            DateOnly birthDate;
            int id = random.Next(00001, 99999);
            while (id.ToString().Length < 5) { id = random.Next(00001, 99999); }
            long unixBirthDateTime;

            try
            {
                birthDate = DateOnly.Parse(birthday);
                unixBirthDateTime = DateTimeOffset.Parse(birthDate.ToString()).ToUnixTimeSeconds();
                if (birthDate.AddDays(14) < DateOnly.FromDateTime(DateTime.Now))
                {
                    await FollowupAsync($"Возможно, игрок {name} больше не новичек, и бесплатный паспорт ему не положен! Оформляю паспорт на месяц...", ephemeral: true);
                    toTime = DateTimeOffset.Now.AddMonths(2);
                }
                else
                {
                    toTime = DateTimeOffset.Now.AddDays(14);
                }
            }
            catch
            {
                await FollowupAsync($"Возможно, с датой `{modal.Birthday}` какая-то ошибка, попробуйте такой тип: 14.02.2023", ephemeral: true);
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

            Passport passport = new()
            {
                Employee = user.Id,
                RpName = RpName,
                Gender = gender,
                Date = toTime.ToUnixTimeSeconds(),
                birthDate = unixBirthDateTime,
                Applicant = name,
                Id = id,
                Support = supporter
            };
            Reports report = new()
            {
                Employee = Startup.sp.GetUser(Context.User.Id.ToString()).Result.Name,
                type = Types.ReportTypes.editPassport
            };
            await Startup.appDbContext.Reports.AddAsync(report);

            if (Startup.appDbContext.Passport.FindAsync(passport.Id).Result != null)
            {
                bool isUnical = false;
                while (!isUnical)
                {
                    id = random.Next(00001, 99999);
                    while (id.ToString().Length < 5) { id = random.Next(00001, 99999); }
                    passport.Id = id;
                    Console.WriteLine(passport.Id);
                    if (Startup.appDbContext.Passport.FindAsync(passport.Id).Result == null) { break; }
                }
            }

            var passportData = new EmbedFieldBuilder()
                .WithName("Данные паспорта:")
                .WithValue(@$"
Имя: {passport.Applicant}
РП Имя: {passport.RpName}
Айди: {id}
Благотворитель: {passport.Support}
Гендер: {passport.Gender}
Дата рождения: <t:{passport.birthDate}:D>").WithIsInline(true);

            var author = new EmbedAuthorBuilder()
                .WithName(user.DisplayName)
                .WithIconUrl(user.GetDisplayAvatarUrl());

            var embed = new EmbedBuilder()
                .WithTitle("**Паспорт переделан**")
                .AddField(passportData)
                .AddField(new EmbedFieldBuilder().WithName("Составитель: ").WithValue(user.GlobalName).WithIsInline(true))
                .AddField(new EmbedFieldBuilder().WithName("Доступен до: ").WithValue($"<t:{toTime.ToUnixTimeSeconds()}:D>").WithIsInline(true))
                .WithThumbnailUrl(spUser.GetSkinPart(SkinPart.face))
                .WithColor(Color.DarkBlue)
                .WithAuthor(author)
                .WithTimestamp(toTime)
                .Build();

            await Startup.appDbContext.Passport.AddAsync(passport);
            await Startup.appDbContext.SaveChangesAsync();
            await FollowupAsync($"ID для паспорта: {id}", embed: embed, ephemeral: true);

            var channel = Context.Guild.GetChannel(1108006685626355733) as ITextChannel;

            var message = await channel.SendMessageAsync(embed: embed);

        }
        [ModalInteraction("passportModal")]
        public async Task createPassportInteraction(INewPassportModal modal)
        {
            await DeferAsync(true);
            string name = modal.NickName;
            string RpName = modal.RPName;
            int supporterInt = modal.Supporter;
            string birthday = modal.Birthday;
            string gender = modal.Gender;

            SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
            Random random = new();
            Supporter supporter;
            User spUser;
            DateTimeOffset toTime;
            DateOnly birthDate;
            long unixBirthDateTime;

            try
            {
                spUser = await User.CreateUser(name);
            }
            catch
            {
                await FollowupAsync("Игрок с таким ником не найден!", ephemeral: true);
                return;
            }

            int id = random.Next(00001, 99999);
            while (id.ToString().Length < 5) { id = random.Next(00001, 99999); }

            try
            {
                birthDate = DateOnly.Parse(birthday);
                unixBirthDateTime = DateTimeOffset.Parse(birthDate.ToString()).ToUnixTimeSeconds();
                if (birthDate.AddDays(14) < DateOnly.FromDateTime(DateTime.Now))
                {
                    await FollowupAsync($"Возможно, игрок {name} играет больше двух недель, и бесплатный паспорт ему не положен! Оформляю паспорт на два месяца...", ephemeral: true);
                    toTime = DateTimeOffset.Now.AddMonths(2);
                }
                else
                {
                    toTime = DateTimeOffset.Now.AddDays(14);
                }
            }
            catch
            {
                await FollowupAsync($"Возможно, с датой `{modal.Birthday}` какая-то ошибка, попробуйте такой тип: 14.02.2023", ephemeral: true);
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

            Passport passport = new()
            {
                Employee = user.Id,
                RpName = RpName,
                Gender = gender,
                Date = DateTimeOffset.Now.ToUnixTimeSeconds(),
                birthDate = unixBirthDateTime,
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
                    while (id.ToString().Length < 5) { id = random.Next(00001, 99999); }
                    if (Startup.appDbContext.Passport.FindAsync(passport.Id).Result == null) { break; }
                }
            }

            var passportData = new EmbedFieldBuilder()
                .WithName("Данные паспорта:")
                .WithValue(@$"
Имя: {passport.Applicant}
РП Имя: {passport.RpName}
Айди: {id}
Благотворитель: {(int)passport.Support }
Гендер: {passport.Gender}
Дата рождения: <t:{passport.birthDate}:D>")
                .WithIsInline(true);

            var author = new EmbedAuthorBuilder()
                .WithName(user.DisplayName)
                .WithIconUrl(user.GetDisplayAvatarUrl());

            var embed = new EmbedBuilder()
                .WithTitle("**Новый паспорт**")
                .AddField(passportData)
                .AddField(new EmbedFieldBuilder().WithName("Составитель: ").WithValue(user.GlobalName).WithIsInline(true))
                .AddField(new EmbedFieldBuilder().WithName("Доступен до: ").WithValue($"<t:{toTime.ToUnixTimeSeconds()}:D>").WithIsInline(true))
                .WithThumbnailUrl(spUser.GetSkinPart(SkinPart.face))
                .WithColor(Color.DarkBlue)
                .WithAuthor(author)
                .WithTimestamp(toTime)
                .Build();

            Reports report = new()
            {
                Employee = ((IGuildUser)Context.User).DisplayName,
                type = Types.ReportTypes.NewPassport
            };
            await Startup.appDbContext.Reports.AddAsync(report);
            await Startup.appDbContext.Passport.AddAsync(passport);
            if (!RpName.StartsWith("test")) { await Startup.appDbContext.SaveChangesAsync(); }
            await FollowupAsync($"ID для паспорта: {id}", embed: embed, ephemeral: true);

            var channel = Context.Guild.GetChannel(1108006685626355733) as ITextChannel;

            await channel.SendMessageAsync(embed: embed);
        }
    }
}

