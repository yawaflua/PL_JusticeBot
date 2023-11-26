using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApp.Database.Tables;
using DiscordApp.Enums;
using spworlds.Types;
using DiscordApp.Justice.Modals;
using System;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Hosting;
using DiscordApp.Types;
using Newtonsoft.Json;

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
        [ComponentInteraction("BuyIdPassportModal")]
        public async Task buyIdPassport() => await Context.Interaction.RespondWithModalAsync<INewIdPassportModal>("buyPassportId");

        [ComponentInteraction("reBuyIdPassportModal")]
        public async Task reworkPassportWithId() => await Context.Interaction.RespondWithModalAsync<IReNewPassportWithIdModal>("renewPassportWithIdModal");

        [ModalInteraction("buyPassportId")]
        public async Task createPassportInteraction(INewIdPassportModal modal)
        {
            await DeferAsync(true);

            string name = modal.NickName;
            string RpName = modal.RPName;
            int supporterInt = modal.Supporter;

            string gender = modal.Gender;

            Startup startup = new();

            SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
            Supporter supporter;
            Random random = new();
            spworlds.Types.User spUser = await spworlds.Types.User.CreateUser(name);
            var spUserData = await startup.getUserData(name);
            DateTimeOffset toTime;
            DateOnly birthDate;
            int id;
            long unixBirthDateTime;
            string cityName;
            string cardNumber;
            bool isIntNewPassportId = int.TryParse(modal.newId, out id);
            if (!isIntNewPassportId)
            {
                await FollowupAsync("Новый номер паспорта это не число!", ephemeral: true);
                return;
            }
            if (modal.newId[0] == '0')
            {
                await FollowupAsync("Первой цифрой не может быть 0", ephemeral: true);
                return;
            }
            if (Startup.appDbContext.Passport.FirstOrDefault(k => k.Id == id) != null)
            {
                await FollowupAsync("Паспорт с таким числом уже есть!", ephemeral: true);
                return;
            }
            try
            {
                birthDate = DateOnly.FromDateTime(spUserData.createdAt);
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
                await FollowupAsync($"Сайт вернул очень странную дату... Попробуйте позже, и напишите об этом <@945317832290336798>", ephemeral: true);
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
            if (spUserData.city != null)
            {
                cityName = spUserData.city.name;
            }
            else
            {
                cityName = "Спавн";
            }
            if (spUserData.cardsOwned.Count > 0)
            {
                cardNumber = spUserData.cardsOwned.First().number;
            }
            else
            {
                cardNumber = "Отсутствует";
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


            var passportData = new EmbedFieldBuilder()
                .WithName("Данные паспорта:")
                .WithValue(@$"
Имя: {passport.Applicant}
РП Имя: {passport.RpName}
Айди: {id}
Благотворитель: {passport.Support}
Гендер: {passport.Gender}
Дата рождения: <t:{passport.birthDate}:D>
Город: {cityName}
Номер карты: {cardNumber}")
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
                type = ReportTypes.buyId
            };
            await Startup.appDbContext.Reports.AddAsync(report);
            await Startup.appDbContext.Passport.AddAsync(passport);
            if (!RpName.StartsWith("test")) { await Startup.appDbContext.SaveChangesAsync(); }
            await FollowupAsync($"ID для паспорта: {id}", embed: embed, ephemeral: true);

            var channel = Context.Guild.GetChannel(1108006685626355733) as ITextChannel;

            await channel.SendMessageAsync(embed: embed);
        }

        [ModalInteraction("ReNewPassportWithIdModal")]
        public async Task reCreatePassportIdInteraction(IReNewPassportWithIdModal modal)
        {
            await DeferAsync(true);

            int passportId;
            int newId;
            bool tryToParsePassport = int.TryParse(modal.Id, out passportId);
            bool tryToParseNewId = int.TryParse(modal.newId, out newId);
            if (!tryToParsePassport || !tryToParseNewId) { await FollowupAsync("Айди паспорта устаревший или новый айди не число, используйте кнопку \"Создать новый\" для создания паспорта", ephemeral: true); return; }
            var passport = Startup.appDbContext.Passport.Where(x => x.Id == passportId).FirstOrDefault();
            if (passport == null ) { await FollowupAsync("ID паспорта не правильный, или не существует.", ephemeral: true); return; }
            if (modal.newId[0] == '0') { await FollowupAsync("Такое нельзя, первая цифра 0 - бан", ephemeral: true); return; }
            Startup startup = new();
            SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
            Random random = new();
            var spUser = await spworlds.Types.User.CreateUser(passport.Applicant);
            var spUserData = await startup.getUserData(passport.Applicant);
            string cityName;
            string cardNumber;
            DateTimeOffset toTime;
            if (DateTimeOffset.FromUnixTimeSeconds(passport.birthDate).AddDays(14) < DateTimeOffset.Now)
            {
                toTime = DateTime.Now.AddDays(_AddDays);
            }
            else
            {
                toTime = DateTime.Now.AddDays(14);
            }

            if (spUserData.city != null)
            {
                cityName = spUserData.city.name;
            }
            else
            {
                cityName = "Спавн";
            }
            if (spUserData.cardsOwned.Count > 0)
            {
                cardNumber = spUserData.cardsOwned.First().number;
            }
            else
            {
                cardNumber = "Отсутствует";
            }

            int id = newId;
            long unixTime = toTime.ToUnixTimeSeconds();
            long nowUnixTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            passport.Id = id;
            passport.Date = nowUnixTime;

            var report = new Reports()
            {
                Employee = Startup.sp.GetUser(Context.User.Id.ToString()).Result.Name,
                type = ReportTypes.ChangePassportId
            };

            var passportData = new EmbedFieldBuilder()
                .WithName("Данные паспорта:")
                .WithValue(@$"
Имя: {passport.Applicant}
РП Имя: {passport.RpName}
Айди: {id}
Благотворитель: {passport.Support}
Гендер: {passport.Gender}
Дата рождения: <t:{passport.birthDate}:D>
Город: {cityName}
Номер карты: {cardNumber}").WithIsInline(true);

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
                await FollowupAsync("Паспорт с таким ID существует!", ephemeral: true);
                return;
            }
            await Startup.appDbContext.Reports.AddAsync(report);
            await Startup.appDbContext.Passport.AddAsync(passport);
            await Startup.appDbContext.SaveChangesAsync();
            await FollowupAsync($"ID для паспорта: {id}", embed: embed, ephemeral: true);

            var channel = Context.Guild.GetChannel(1108006685626355733) as ITextChannel;

            var message = await channel.SendMessageAsync(embed: embed);

            
        }


        [ComponentInteraction("BuyIdPassportButton")]
        public async Task buyIdPassportButton() 
        {
            await DeferAsync(true);
            var Embed = new EmbedBuilder()
                .WithTitle("**Выбери тип паспоррта!**")
                .WithDescription("Только при покупке номера")
                .WithColor(Color.Blue)
                .Build();
            var Components = new ComponentBuilder()
                .WithButton(new ButtonBuilder() { CustomId = "BuyIdPassportModal", Label = "Новый паспорт", Style = ButtonStyle.Primary })
                .WithButton(new ButtonBuilder() { CustomId = "reBuyIdPassportModal", Label = "Замена паспорта", Style = ButtonStyle.Primary })
                .Build();
            await FollowupAsync(embed: Embed, components: Components, ephemeral:true);
        }

        [ModalInteraction("reworkpassportModal")]
        public async Task reCreatePassportInteraction(IReWorkPassportModal modal)
        {
            await DeferAsync(true);

            int passportId;
            bool tryToParsePassport = int.TryParse(modal.Id, out passportId);
            bool recreatePassport = modal.IsNewPassport == 1;
            if (recreatePassport)
            {
                await FollowupAsync("Нажмите на кнопку ниже", components: new ComponentBuilder().WithButton(new ButtonBuilder("Кнопочка", "reNewPassportButton")).Build(), ephemeral: true);
            }
            else
            { 
                if (!tryToParsePassport) { await FollowupAsync("Айди паспорта устаревший, используйте кнопку \"Создать новый\" для создания паспорта", ephemeral: true); return; }
                var passport = Startup.appDbContext.Passport.Where(x => x.Id == passportId).FirstOrDefault();
                if (passport == null) { await FollowupAsync("ID паспорта не правильный, или не существует.", ephemeral: true); return; }

                Startup startup = new();
                SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
                Random random = new();
                var spUser = await spworlds.Types.User.CreateUser(passport.Applicant);
                var spUserData = await startup.getUserData(passport.Applicant);
                string cityName;
                string cardNumber;
                DateTimeOffset toTime;
                if (DateTimeOffset.FromUnixTimeSeconds(passport.birthDate).AddDays(14) < DateTimeOffset.Now)
                {
                    toTime = DateTime.Now.AddDays(_AddDays);
                }
                else
                {
                    toTime = DateTime.Now.AddDays(14);
                }

                if (spUserData.city != null)
                {
                    cityName = spUserData.city.name;
                }
                else
                {
                    cityName = "Спавн";
                }
                if (spUserData.cardsOwned.Count > 0)
                {
                    cardNumber = spUserData.cardsOwned.First().number;
                }
                else
                {
                    cardNumber = "Отсутствует";
                }

                int id = random.Next(10000, 99999);
                while (id.ToString().Length < 5) { id = random.Next(10000, 99999); }
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
Дата рождения: <t:{passport.birthDate}:D>
Город: {cityName}
Номер карты: {cardNumber}").WithIsInline(true);

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
                        id = random.Next(10000, 99999);
                        passport.Id = id;
                        while (id.ToString().Length < 5) { id = random.Next(10000, 99999); }
                        Console.WriteLine(passport.Id);
                        if (Startup.appDbContext.Passport.FindAsync(passport.Id).Result == null) { break; }
                    }
                }

                var report = new Reports()
                {
                    Employee = Startup.sp.GetUser(Context.User.Id.ToString()).Result.Name,
                    type = ReportTypes.editPassport
                };

                await Startup.appDbContext.Reports.AddAsync(report);
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
            string gender = modal.Gender;

            Startup startup = new ();

            SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
            Supporter supporter;
            Random random = new();
            spworlds.Types.User spUser = await spworlds.Types.User.CreateUser(name);
            var spUserData = await startup.getUserData(name);
            DateTimeOffset toTime;
            DateOnly birthDate;
            int id;
            Utilities.Utilities.IdGenerator(out id);
            long unixBirthDateTime;
            string cityName;
            string cardNumber;
            try
            {
                birthDate = DateOnly.FromDateTime(spUserData.createdAt);
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
                await FollowupAsync($"Сайт вернул очень странную дату... Попробуйте позже, и напишите об этом <@945317832290336798>", ephemeral: true);
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
            if (spUserData.city != null)
            {
                cityName = spUserData.city.name;
            }
            else
            {
                cityName = "Спавн";
            }
            if (spUserData.cardsOwned.Count > 0)
            {
                cardNumber = spUserData.cardsOwned.First().number;
            }
            else
            {
                cardNumber = "Отсутствует";
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
                type = ReportTypes.editPassport
            };
            await Startup.appDbContext.Reports.AddAsync(report);

            if (Startup.appDbContext.Passport.FindAsync(passport.Id).Result != null)
            {
                bool isUnical = false;
                while (!isUnical)
                {
                    Utilities.Utilities.IdGenerator(out id);
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
Благотворитель: {(int)passport.Support}
Гендер: {passport.Gender}
Дата рождения: <t:{passport.birthDate}:D>
Город: {cityName}
Номер карты: {cardNumber}").WithIsInline(true);



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

            string gender = modal.Gender;

            Startup startup = new();

            SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
            Supporter supporter;
            Random random = new();
            spworlds.Types.User spUser = await spworlds.Types.User.CreateUser(name);
            var spUserData = await startup.getUserData(name);
            DateTimeOffset toTime;
            DateOnly birthDate;
            int id = random.Next(10000, 99999);
            while (id.ToString().Length < 5) { id = random.Next(10000, 99999); }
            long unixBirthDateTime;
            string cityName;
            string cardNumber;
            try
            {
                birthDate = DateOnly.FromDateTime(spUserData.createdAt);
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
                await FollowupAsync($"Сайт вернул очень странную дату... Попробуйте позже, и напишите об этом <@945317832290336798>", ephemeral: true);
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
            if (spUserData.city != null)
            {
                cityName = spUserData.city.name;
            }
            else
            {
                cityName = "Спавн";
            }
            if (spUserData.cardsOwned.Count > 0)
            {
                cardNumber = spUserData.cardsOwned.First().number;
            }
            else
            {
                cardNumber = "Отсутствует";
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

            if (Startup.appDbContext.Passport.FindAsync(passport.Id).Result != null)
            {
                bool isUnical = false;
                while (!isUnical)
                {
                    id = random.Next(10000, 99999);
                    while (id.ToString().Length < 5) { id = random.Next(10000, 99999); }
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
Дата рождения: <t:{passport.birthDate}:D>
Город: {cityName}
Номер карты: {cardNumber}")
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
                type = ReportTypes.NewPassport
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

