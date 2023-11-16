namespace DiscordApp.Types
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class A
    {
        public string name { get; set; }
        public string type { get; set; }
        public string @class { get; set; }
        public int ttl { get; set; }
        public int rdlength { get; set; }
        public string rdata { get; set; }
        public string cname { get; set; }
        public string address { get; set; }
    }

    public class Debug
    {
        public bool ping { get; set; }
        public bool query { get; set; }
        public bool srv { get; set; }
        public bool querymismatch { get; set; }
        public bool ipinsrv { get; set; }
        public bool cnameinsrv { get; set; }
        public bool animatedmotd { get; set; }
        public bool cachehit { get; set; }
        public int cachetime { get; set; }
        public int cacheexpire { get; set; }
        public int apiversion { get; set; }
        public Dns dns { get; set; }
        public Error error { get; set; }
    }

    public class Dns
    {
        public Error error { get; set; }
        public List<A> a { get; set; }
    }

    public class Error
    {
        public Srv srv { get; set; }
        public string query { get; set; }
    }

    public class List
    {
        public string name { get; set; }
        public string uuid { get; set; }
    }

    public class Motd
    {
        public List<string> raw { get; set; }
        public List<string> clean { get; set; }
        public List<string> html { get; set; }
    }

    public class Players
    {
        public int online { get; set; }
        public int max { get; set; }
        public List<List> list { get; set; }
    }

    public class Protocol
    {
        public int version { get; set; }
        public string name { get; set; }
    }

    public class ServerResponse
    {
        public string ip { get; set; }
        public int port { get; set; }
        public Debug debug { get; set; }
        public Motd motd { get; set; }
        public Players players { get; set; }
        public string version { get; set; }
        public bool online { get; set; }
        public Protocol protocol { get; set; }
        public string hostname { get; set; }
        public string icon { get; set; }
        public string software { get; set; }
        public bool eula_blocked { get; set; }
    }

    public class Srv
    {
        public string hostname { get; set; }
        public string message { get; set; }
    }

}
