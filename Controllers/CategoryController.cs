using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GroupProject.Models;
using GroupProject.ViewModels;

namespace GroupProject.Controllers;

public class CategoryController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var categories = await context.Categories!.ToListAsync();
        var vm = new CategoryIndexVm { Categories = categories };
        return View(vm);
    }

    public IActionResult Create()
    {
        var vm = new CategoryCreateVm
        {
            Category = new Category()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CategoryCreateVm categoryVm)
    {
        if(ModelState.IsValid)
        {
            context.Add(categoryVm.Category);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(categoryVm);
    }
}