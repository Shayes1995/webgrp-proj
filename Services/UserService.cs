using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using GroupProject.Models;

namespace GroupProject.Services;

public class UserService(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
{

    public async Task<bool> IsAdminAsync()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return false;
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        return await userManager.IsInRoleAsync(user, RoleConstants.Admin);
    }

    public async Task<bool> IsNormalUserAsync()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return false;
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        return await userManager.IsInRoleAsync(user, RoleConstants.User);
    }
}