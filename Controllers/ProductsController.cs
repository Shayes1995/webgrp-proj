using GroupProject.Models;
using GroupProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GroupProject.Controllers;

public class ProductsController(ApplicationDbContext context) : Controller
{

    
    public async Task<IActionResult> Index()
    {
        var products = await context.Products!
        .Include(p => p.Category).ToListAsync();

        var vm = new ProductsIndexVm { Product = products };
        return View(vm);

        // var products = new List<Product>
        // {
        //     new() {
        //         Id = 1,
        //         Name = "Potatis",
        //         Price = 4,
        //         Description = "aaa",
        //         CategoryId = 1,
        //     },
        //     new() {
        //         Id = 2,
        //         Name = "Ostbågar",
        //         Price = 49,
        //         Description = "bbb",
        //         CategoryId = 2,
        //     }
        // };

        // var vm = new ProductsIndexVm { Product = products};
        // return View(vm);
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var vm = new ProductCreateVm
        {
            Product = new Product(),
            Categories = await context.Categories!.ToListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVm productVm)
    {
        if (ModelState.IsValid)
        {
            context.Add(productVm.Product);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        productVm.Categories = await context.Categories!.ToListAsync();
        return View(productVm);
    }

    public async Task<IActionResult> EditAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Products!.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var categories = await context.Categories!.ToListAsync();

        var vm = new ProductsEditVm
        {
            Product = product,
            Categories = categories
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> Edit(ProductsEditVm productVm)
    {
        var product = productVm.Product;
        if (ModelState.IsValid)
        {
            if (!ProductExists(product.Id))
            {
                return NotFound();
            }

            context.Update(product);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(productVm);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Products!
        .FirstOrDefaultAsync(m => m.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var vm = new ProductDeleteVm
        {
            Product = product
        };
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var product = await context.Products!.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        context.Products!.Remove(product);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool ProductExists(int id)
    {
        return context.Products!.Any(e => e.Id == id);
    }
}