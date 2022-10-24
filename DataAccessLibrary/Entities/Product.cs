using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Name must be less than 100 characters.")]
    public string Name { get; set; }

    [MaxLength(500, ErrorMessage = "Description must be less than 500 characters.")]
    public string Description { get; set; }

    [Required]
    [Range(1, 99999, ErrorMessage = "List price maximum is $10000.")]
    [Column(TypeName = "decimal(7,2)")]
    [DisplayName("List price")]
    public decimal ListPrice { get; set; }

    [Required]
    [Range(1, 99999, ErrorMessage = "Actual price maximum is $10000.")]
    [Column(TypeName = "decimal(7,2)")]
    [DisplayName("Actual price")]
    public decimal ActualPrice { get; set; }

    [MaxLength(200, ErrorMessage = "ImageURL must be less than 200 characters.")]
    [DisplayName("Image URL")]
    public string ImageUrl { get; set; }

    public Category Category { get; set; }

    public int CategoryId { get; set; }
}
