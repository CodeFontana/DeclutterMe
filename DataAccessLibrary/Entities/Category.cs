using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Entities;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [DisplayName("Display Order")]
    [Range(1, 100, ErrorMessage = "Display order must be between 1 and 100, only.")]
    public int DisplayOrder { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
}
