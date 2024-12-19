using GroupProject.Models;
using System.Collections.Generic;

namespace GroupProject.ViewModels;

public class UserListCreateVm
{
    public UserShoppingList UserShoppingList { get; set; } = new UserShoppingList();
    public List<Product> Products { get; set; } = new List<Product>();
}
