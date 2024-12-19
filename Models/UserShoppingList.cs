using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace GroupProject.Models;


public class UserShoppingList
{
    public int Id {get; set;}

    public string UserId {get; set;} = "";

    public IdentityUser? User {get; set;} 

    public int ProductId {get; set;}

    public Product? Product {get; set;}

    public int Quantity {get; set;} = 1;
}