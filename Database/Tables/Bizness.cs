using DiscordApp.Types;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApp.Database.Tables
{
    [Table("Bizness", Schema = "public")]
    public class Bizness
    {
        [Key]
        public int Id { get; set; }
        public Passport Applicant { get; set; }
        public string Employee { get; set; }
        public string BiznessName { get; set; }
        public int[] BiznessEmployes { get; set; }
        public long Date { get; set; }
        public string BiznessType { get; set; }
        public int CardNumber { get; set; }
    }
}
