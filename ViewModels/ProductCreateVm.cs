using GroupProject.Models;

namespace GroupProject.ViewModels;

public class ProductCreateVm
{
    public required Product Product { get; set; } = new();

    public List<Category> Categories { get; set; } = new();

}