using DiscordApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApp.Database.Tables
{
    [Table("ArtsPatent", Schema = "public")]
    public class ArtsPatents
    {
        [Key]
        public int Id { get; set; }
        public string Employee { get; set; }
        public Passport passport { get; set; }
        public long Date { get; set; }
        public int[] Number { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public bool isAllowedToResell { get; set; }
        

    }
}