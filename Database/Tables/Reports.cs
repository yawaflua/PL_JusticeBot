using DiscordApp.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApp.Database.Tables
{
    [Table("Reports", Schema = "public")]
    public class Reports
    {
        [Key]
        public int Id { get; set; }
        public string Employee { get; set; }
        public ReportTypes type { get; set; }

    }
}
