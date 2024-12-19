using GroupProject.Models;
using GroupProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GroupProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userShoppingList = await context.UserShoppingLists!
                .Include(uls => uls.Product)
                .ThenInclude(p => p.Category)
                .Where(uls => uls.UserId == user.Id)
                .ToListAsync();

            var vm = new UserListIndexVm
            {
                UserShoppingLists = userShoppingList
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddToMyList(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            var existingItem = await context.UserShoppingLists!
                .FirstOrDefaultAsync(uls => uls.UserId == userId && uls.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var item = new UserShoppingList
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = 1
                };

                context.UserShoppingLists!.Add(item);
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            var item = await context.UserShoppingLists!
                .FirstOrDefaultAsync(uls => uls.UserId == userId && uls.ProductId == productId);

            if (item == null)
            {
                return NotFound();
            }

            if (item.Quantity > 1)
            {
                item.Quantity--;
            }
            else
            {
                context.UserShoppingLists!.Remove(item);
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Increase(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            var item = await context.UserShoppingLists!
                .Where(uls => uls.UserId == userId && uls.ProductId == productId)
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            item.Quantity++;
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            var item = await context.UserShoppingLists!
                .Where(uls => uls.UserId == userId && uls.ProductId == productId)
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            context.UserShoppingLists!.Remove(item);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}