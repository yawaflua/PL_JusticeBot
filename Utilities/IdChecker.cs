using DiscordApp.Database.Tables;

namespace DiscordApp.Utilities
{
    public class Utilities
    {
        public static void IdGenerator(out int id)
        {
            Random random = new();
            id = random.Next(10000, 99999);
            while (id.ToString().Length < 5) { id = random.Next(10000, 99999); }
            return;
        }
        public static bool IsPassport(int id, out Passport passport)
        {
            passport = Startup.appDbContext.Passport.FirstOrDefault(x => x.Id == id);
            return passport == null;
        }
    }
}
