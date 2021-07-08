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
using System.Collections.ObjectModel;

namespace GGus.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.Product.Include(p => p.Category);
                return View(await applicationDbContext.ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchPtable(string query)
        {
            try
            {
                var applicationDbContext = _context.Product.Include(p => p.Category);
                return PartialView(await applicationDbContext.Where(p => p.Name.Contains(query)).ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }


        //Search Product
        public async Task<IActionResult> Search(string productName, string price, string category)
        {
            try
            {
                int p = Int32.Parse(price);
                var applicationDbContext = _context.Product.Include(a => a.Category).Where(a => a.Name.Contains(productName) && a.Category.Name.Equals(category) && a.Price <= p);
                return View("searchlist", await applicationDbContext.ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                var product = await _context.Product
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                return View(product);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CompanyName,Price,CategoryId,PhotosUrl1,PhotosUrl2,PhotosUrl3,PhotosUrl4,Details,TrailerUrl,PublishDate")] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", product.CategoryId);
                return View(product);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                var product = await _context.Product.FindAsync(id);
                if (product == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }
                ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", product.CategoryId);
                return View(product);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CompanyName,Price,CategoryId,PhotosUrl1,PhotosUrl2,PhotosUrl3,PhotosUrl4,Details,TrailerUrl,PublishDate")] Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(product.Id))
                        {
                            return RedirectToAction("PageNotFound", "Home");
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", product.CategoryId);
                return View(product);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                var product = await _context.Product
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                return View(product);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Product.FindAsync(id);
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Statistics()
        {
            try
            {
                //statistic-1- what is the most "popular" game- the one who appears in most carts
                ICollection<Stat> statistic1 = new Collection<Stat>();
                var result = from p in _context.Product.Include(o => o.Carts)
                             where (p.Carts.Count) > 0
                             orderby (p.Carts.Count) descending
                             select p;
                foreach (var v in result)
                {
                    statistic1.Add(new Stat(v.Name, v.Carts.Count()));
                }

                ViewBag.data1 = statistic1;

                //finish first statistic
                //statistic 2-what is the most common age of the users
                ICollection<Stat> statistic2 = new Collection<Stat>();
                List<User> users = _context.User.ToList();
                int currentYear = DateTime.Today.Year;
                Dictionary<int, int> result2 = new Dictionary<int, int>();
                foreach (User item in users)
                {
                    if (!result2.ContainsKey(currentYear - item.Age.Year))
                    {
                        result2.Add(currentYear - item.Age.Year, 1);
                    }
                    else
                    {
                        int count = result2.GetValueOrDefault(currentYear - item.Age.Year) + 1;
                        result2.Remove(currentYear - item.Age.Year);
                        result2.Add(currentYear - item.Age.Year, count);
                    }

                }

                foreach (var v in result2.OrderBy(k => k.Key))
                {
                    if (v.Value > 0)
                    {
                        statistic2.Add(new Stat(v.Key.ToString(), v.Value));
                    }
                }


                ViewBag.data2 = statistic2;



                //statistic-3- what category hava the biggest number of games
                ICollection<Stat> statistic3 = new Collection<Stat>();
                List<Product> products = _context.Product.ToList();
                List<Category> categories = _context.Category.ToList();
                var result3 = from prod in products
                              join cat in categories on prod.CategoryId equals cat.Id
                              group cat by cat.Id into G
                              select new { id = G.Key, num = G.Count() };

                var porqua = from popo in result3
                             join cat in categories on popo.id equals cat.Id
                             select new { category = cat.Name, count = popo.num };
                foreach (var v in porqua)
                {
                    if (v.count > 0)
                        statistic3.Add(new Stat(v.category, v.count));
                }

                ViewBag.data3 = statistic3;
                return View();
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
            
    

    }

}

    public class Stat
    {
        public string Key;
        public int Values;
        public Stat(string key, int values)
        {
            Key = key;
            Values = values;
        }
    }
   

    

