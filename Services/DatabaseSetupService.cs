using Microsoft.AspNetCore.Identity;
namespace GroupProject.Services;

public class DatabaseSetupService(
    RoleManager<IdentityRole> roleManager,
    UserManager<IdentityUser> userManager)
{
    public async Task InitializeAsync()
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var result = await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create role");
            }
        }

        var adminUser = new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com" };
        var user = await userManager.FindByEmailAsync(adminUser.Email);
        if (user == null)
        {
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create user");
            }
            await userManager.AddToRoleAsync(adminUser, "Admin");

        }
    }

}
