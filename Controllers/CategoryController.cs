using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GroupProject.Models;
using GroupProject.ViewModels;

namespace GroupProject.Controllers;

public class CategoryController(ApplicationDbContext context) : Controller
{

    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "Categories";
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
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> CreateAsync(CategoryCreateVm categoryVm)
    {
        if (ModelState.IsValid)
        {
            context.Add(categoryVm.Category);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(categoryVm);
    }

    public async Task<IActionResult> EditAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await context.Categories!.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var vm = new CategoryEditVm
        {
            Category = category
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> Edit(CategoryEditVm categoryVm)
    {
        var category = categoryVm.Category;
        if (ModelState.IsValid)
        {
            if (!CategoryExists(category.Id))
            {
                return NotFound();
            }

            context.Update(category);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(categoryVm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await context.Categories!
        .FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        var vm = new CategoryDeleteVm
        {
            Category = category
        };

        return View(vm);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var category = await context.Categories!.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        context.Categories!.Remove(category);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    private bool CategoryExists(int id)
    {
        return context.Categories!.Any(e => e.Id == id);
    }
}