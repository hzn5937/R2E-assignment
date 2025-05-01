using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Domain.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
