using DiscordApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApp.Database.Tables
{
    [Table("Passport", Schema = "public")]
    public class Passport
    {
        [Key]
        public int Id { get; set; }
        public ulong Employee { get; set; }
        public string Applicant { get; set; }
        public long Date { get; set; }
        public Supporter Support { get; set; }
        public string Gender { get; set; }
        public string RpName { get; set; }

    }
}