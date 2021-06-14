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
using System.Security.Claims;

namespace GGus.Web.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Carts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {   
            var applicationDbContext = _context.Cart.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

       
        public async Task<IActionResult> Search(string query)
        {
            var userName = User.Identity.Name;

            var user = _context.User.FirstOrDefault(x => x.Username == userName);

            var cart = _context.Cart.FirstOrDefault(c => c.UserId == user.Id);

            List<Product> products = (List<Product>)cart.Products.Where(p => p.Name.Contains(query) || p.Details.Contains(query) || query == null);
            //var userId = _context.Cart.Where(x => x.User.Username == userName);


            // var cart = _context.Cart.Where(x => x.UserId = userId);
            // var applicationDbContext = _context.Cart.Include(x => x.User);
            return View("Index",  products);
            //return View();
        }

        private IActionResult View(string v, object p)
        {
            throw new NotImplementedException();
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TotalPrice")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", cart.UserId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", cart.UserId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TotalPrice")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", cart.UserId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
        // GET: Carts/Details
        public ActionResult MyCart()
        {
            ViewData["products"] = _context.Product;
            return View();
        }

        [HttpPost, ActionName("AddToCart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id)
        {
            Product product = _context.Product.FirstOrDefault(x => x.Id == id);
            String userName = HttpContext.User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            if (user.Cart.Products == null)
                user.Cart.Products = new List<Product>();
            user.Cart.Products.Add(product);
            user.Cart.TotalPrice += product.Price;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyCart));
        }
    }
}
