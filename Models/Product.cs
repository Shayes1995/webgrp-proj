namespace GroupProject.Models;

public class Product
{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public decimal Price {get; set;} = 0;
    public string Description {get; set;} = "";

    public int CategoryId {get; set;}

    public Category? Category {get; set;}
}