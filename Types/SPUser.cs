namespace DiscordApp.Types
{
    public class CardsOwned
    {
        public string name { get; set; }
        public int color { get; set; }
        public string number { get; set; }
        public string id { get; set; }
    }

    public class City
    {
        public Mayor mayor { get; set; }
        public string name { get; set; }
        public int x { get; set; }
        public int z { get; set; }
    }

    public class Mayor
    {
        public string id { get; set; }
    }

    public class Root
    {
        public string id { get; set; }
        public bool isBanned { get; set; }
        public User user { get; set; }
        public List<string> roles { get; set; }
        public City? city { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
        public List<CardsOwned> cardsOwned { get; set; }
        public bool isFollowed { get; set; }
        public bool isFollowingYou { get; set; }
    }

    public class User
    {
        public bool isAdmin { get; set; }
        public string minecraftUUID { get; set; }
        public string username { get; set; }
    }
}
