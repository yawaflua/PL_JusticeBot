namespace DiscordApp;

public class Program
{
    static void Main()
    {
        /**
        Discord.Discord discord = new("MTE1Mjk0Njg2NTMwNjg4MjExOA.GL4wd6.XXhH5cam_zhxBCGYKJI-Z1IfbUQ5J-80H5Jpds");
        Console.WriteLine("Hello! Starts discord bot...");
        discord.InitBot().GetAwaiter().GetResult();
        **/
        Console.WriteLine("Starts Web-API...");
        CreateHostBuilder()
            .Build()
            .Run();

    }
    

    private static IHostBuilder CreateHostBuilder() 
    {
        return Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(webHost => {
            webHost.UseStartup<Startup>();
            webHost.UseKestrel(kestrelOptions => { kestrelOptions.ListenAnyIP(80); });
        });
        
    }

    public static bool IsDebug()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }
    
}

