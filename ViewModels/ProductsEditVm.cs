using GroupProject.Models;

namespace GroupProject.ViewModels;

public class ProductsEditVm
{
    public required Product Product { get; set; }
    public List<Category> Categories { get; set; } = new();
}