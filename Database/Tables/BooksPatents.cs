using DiscordApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApp.Database.Tables
{
    [Table("BooksPatent", Schema = "public")]
    public class BooksPatents
    {
        [Key]
        public int Id { get; set; }
        public string Employee { get; set; }
        public Passport passport { get; set; }
        public long Date { get; set; }
        public string Name { get; set; }
        public string Annotation { get; set; }
        public string Janre { get; set; }
        public bool isAllowedToResell { get; set; }


    }
}