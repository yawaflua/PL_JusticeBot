using DiscordApp.Enums;

namespace DiscordApp.Types
{
    public class UserObject
    {
        public ulong Id { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string Avatar { get; set; }
        public bool? Bot { get; set; }
        public bool? MFA_enabled { get; set; }
        public bool? Verified { get; set; }
        public string eMail { get; set; }
        public UserFlags Flags { get; set; }
        public NitroSubscription Premium_type { get; set; }
    }
}
