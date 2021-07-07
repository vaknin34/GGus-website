﻿using System;
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

        // GET: Categories
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Search(string query)
        {
           
            return Json(await _context.Category.Where(c => c.Name.Contains(query)).ToListAsync());
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        public async Task<IActionResult> CommingSoon()
        {

            return View(await _context.Product.Where(x => x.Id % 12 == 0).ToListAsync());
        }

        public async Task<IActionResult> BestSeller()
        {

            return View(await _context.Product.Where(x => x.Id % 12 == 1).ToListAsync());
        }

        public async Task<IActionResult> GoodDeals()
        {

            return View(await _context.Product.Where(x => x.Id % 8 == 2).ToListAsync());
        }

        public async Task<IActionResult> NewGames()
        {

            return View(await _context.Product.Where(x => x.Id % 12 == 3).ToListAsync());
        }

        
        public async Task<IActionResult> AllGames()
        {
            var products =
                from category in _context.Category
                join prod in _context.Product on category.Id equals prod.CategoryId
                orderby category.Id
                select prod;

            return View(await products.ToListAsync());
        }

        [HttpPost]
        public  IActionResult GroupByPrice()
        {
            /*
            var products =
                from category in _context.Category
                join prod in _context.Product on category.Id equals prod.CategoryId
                orderby prod.Price
                select prod;
            */
            var groups = from p in _context.Product.ToList()
            group p by p.Price
            into g
            orderby g.Key
            select g;

            List<Product> products = new List<Product>();
            foreach (var prod in groups) {
                for (int i = 0; i < prod.ToList().Count; i++)
                {
                    products.Add(prod.ElementAt(i));
                }
               }

            return View("AllGames", products);
        }

    }
}




