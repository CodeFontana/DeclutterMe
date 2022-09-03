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
    public double ListPrice { get; set; }

    [Required]
    [Range(1, 10000, ErrorMessage = "Actual price maximum is $10000.")]
    public double ActualPrice { get; set; }

    [MaxLength(1000, ErrorMessage = "ImageURL must be less than 1000 characters.")]
    public string ImageUrl { get; set; }

    public int CategoryId{ get; set; }

    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
}
