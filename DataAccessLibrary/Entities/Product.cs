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
    [Range(1, 10000, ErrorMessage = "List price maximum is $10000.")]
    [DisplayName("List price")]
    public double ListPrice { get; set; }

    [Required]
    [Range(1, 10000, ErrorMessage = "Actual price maximum is $10000.")]
    [DisplayName("Actual price")]
    public double ActualPrice { get; set; }

    [MaxLength(200, ErrorMessage = "ImageURL must be less than 200 characters.")]
    [DisplayName("Image URL")]
    public string ImageUrl { get; set; }

    [DisplayName("Category")]
    public int CategoryId{ get; set; }

    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
}
