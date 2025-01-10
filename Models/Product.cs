using System.ComponentModel.DataAnnotations;
namespace GroupProject.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The product name field is required.")]
    [Display(Name = "Product name")]
    [StringLength(50)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "The price field is required.")]
    [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
    public decimal Price { get; set; } = 0;

    [MaxLength(50)]
    public string? Description { get; set; } = "";

    // [Required(ErrorMessage = "The category field is required.")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public string Imageurl { get; set; } = "";
}