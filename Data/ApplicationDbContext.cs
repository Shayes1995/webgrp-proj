namespace GroupProject.Models; 

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Categories { get; set; }

    public DbSet<UserShoppingList>? UserShoppingLists { get; set; }

}
