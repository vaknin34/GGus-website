using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GGus.Web.Data;
using GGus.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace GGus.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Action()
        {
            
            return View(await _context.Product.Where(x => x.CategoryId == 1).ToListAsync());
        }

        public async Task<IActionResult> Adventure()
        {

            return View(await _context.Product.Where(x => x.CategoryId == 2).ToListAsync());
        }


        public async Task<IActionResult> RPG()
        {

            return View(await _context.Product.Where(x => x.CategoryId == 3).ToListAsync());
        }

        public async Task<IActionResult> Sport()
        {

            return View(await _context.Product.Where(x => x.CategoryId == 4).ToListAsync());
        }

        public async Task<IActionResult> Race()
        {

            return View(await _context.Product.Where(x => x.CategoryId == 5).ToListAsync());
        }

        public async Task<IActionResult> Horror()
        {

            return View(await _context.Product.Where(x => x.CategoryId == 6).ToListAsync());
        }
        public async Task<IActionResult> Puzzle()
        {

            return View(await _context.Product.Where(x => x.CategoryId == 7).ToListAsync());
        }


        public async Task<IActionResult> Simulation()
        {

            return View(await _context.Product.Where(x => x.CategoryId == 8).ToListAsync());
        }



        // GET: Categories
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["products"] = new SelectList(_context.Product.Where(x => x.CategoryId == 0),nameof(Product.Id),nameof(Product.Name));
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category,int[] products)
        {
            if (ModelState.IsValid)
            {
                category.Products = new List<Product>();
                category.Products.AddRange(_context.Product.Where(x => products.Contains(x.Id)));
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
