using DiscordApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApp.Database.Tables
{
    [Table("Patents", Schema = "public")]
    public class Patents
    {
        public string Employee { get; set; }
        public string Applicant { get; set; }
        public int Date { get; set; }
        [Key]
        public int[] Number { get; set; }
        public Supporter Support { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }

    }
}