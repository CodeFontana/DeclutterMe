namespace DataAccessLibrary.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
}
