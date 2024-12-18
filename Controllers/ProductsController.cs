using GroupProject.Models;
using GroupProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vm = new ProductsEditVm
        {
            ProductId = id.Value
        };
        return View(vm);
    }
}