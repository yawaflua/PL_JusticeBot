using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApp.Database.Tables
{
    [Table("Autoreactions", Schema = "public")]
    public class Autoreactions
    {
        [Key]
        public ulong ChannelId { get; set; }
        public string EmoteId { get; set; }

    }
}