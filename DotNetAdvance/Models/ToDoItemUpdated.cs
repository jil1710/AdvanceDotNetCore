using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DotNetAdvance.Models
{
    public class ToDoItemUpdated
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("title")]
        public string Title { get; set; }
        [Required]
        [Column("description")]
        public string Description { get; set; }
        [Required]
        [Column("itemtype")]
        public string Type { get; set; }
        [Required]
        [Column("date")]
        public string Date { get; set; }
    }
}
