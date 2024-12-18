using System.ComponentModel.DataAnnotations;
namespace GroupProject.Models;

public class Product
{
    public int Id {get; set;}
    [Required]
    [StringLength(50)]
    public string Name {get; set;} = "";
    public decimal Price {get; set;} = 0;
    [StringLength(200)]
    public string Description {get; set;} = "";

    public int CategoryId {get; set;}

    public Category? Category {get; set;}
}