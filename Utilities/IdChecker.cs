namespace DiscordApp.Utilities
{
    public class IdChecker
    {
        public static void IdLenghtIsLower(out int id)
        {
            Random random = new();
            id = random.Next(00001, 99999);
            while (id.ToString().Length < 5) { id = random.Next(00001, 99999); }
            return;
        }
    }
}
