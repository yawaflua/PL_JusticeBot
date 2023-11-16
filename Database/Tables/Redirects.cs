using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DiscordApp.Types;

namespace DiscordApp.Database.Tables
{
    [Table("Redirects", Schema = "public")]
    public class Redirects
    {
        [Key]
        public string Id { get; set; }
        public string url { get; set; }
        public RedirectType RedirectType { get; set; } = RedirectType.None;
    }
}
