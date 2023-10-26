using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApp.Database.Tables
{
    [Table("Autobranches", Schema = "public")]
    public class Autobranches
    {
        [Key]
        public ulong ChannelId { get; set; }
        public string BranchName { get; set; }

    }
}