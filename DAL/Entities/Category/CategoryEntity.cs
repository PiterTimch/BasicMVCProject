using System.ComponentModel.DataAnnotations;

namespace DAL.Entities.Category
{
    public class CategoryEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Url]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
