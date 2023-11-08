using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DiscordApp.Database.Tables
{

    [Table("Certificate", Schema = "public")]
    public class Certificate
    {
        [Key]
        public int Id { get; set; }
        public string Employee { get; set; }
        public Passport passport { get; set; }
        public DateOnly Date { get; set; }
        public string Text { get; set; }
        public int DocumentId { get; set; }

        
    }
}
