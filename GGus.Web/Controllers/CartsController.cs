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

        public IActionResult Search(string query)
        {
            var userName = User.Identity.Name;

            var user = _context.User.FirstOrDefault(x => x.Username == userName);

            var cart = _context.Cart.FirstOrDefault(c => c.UserId == user.Id);

            List<Product> products = (List<Product>)cart.Products.Where(p => p.Name.Contains(query) || p.Details.Contains(query) || query == null);

            return View("Index", products);
            //return View();
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

        public IActionResult MyCart()
        {
            String userName = HttpContext.User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            Cart cart = _context.Cart.FirstOrDefault(x => x.UserId == user.Id);
            cart.Products = _context.Product.Where(x => x.Carts.Contains(cart)).ToList();

            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        [HttpPost, ActionName("AddToCart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id) //product id
        {
            Product product = _context.Product.FirstOrDefault(x => x.Id == id);
            String userName = HttpContext.User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            Cart cart = _context.Cart.FirstOrDefault(x => x.UserId == user.Id);
            if (user.Cart.Products == null)
                user.Cart.Products = new List<Product>();
            if (product.Carts == null)
                product.Carts = new List<Cart>();
            user.Cart.Products.Add(product);
            product.Carts.Add(cart);
            user.Cart.TotalPrice += product.Price;
            _context.Update(cart);
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyCart));
        }

        // POST: Carts/removeProduct/5
        [HttpPost, ActionName("RemoveProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            Product product = _context.Product.FirstOrDefault(x => x.Id == id);
            String userName = HttpContext.User.Identity.Name;

            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            Cart cart = _context.Cart.Include(db => db.Products)
                .FirstOrDefault(x => x.UserId == user.Id);

            if (product != null)
            {
                cart.Products.Remove(product);
                cart.TotalPrice -= product.Price;
            }
            //List<Product> cartProducts = _context.Product.Where(x => x.Id != id && x.Carts.Contains(cart)).ToList();
            //cart.Products = cartProducts;


            //cart.Products = _context.Product.Where(x => x.Carts.Contains(cart)).ToList();
            //product.Carts = _context.Cart.Where(x => !x.Products.Contains(product)).ToList();
            //cart.Products.Remove(product);
            //product.Carts.Remove(cart);
            //_context.Cart.Update(cart);
            //_context.Product.Update(product);
            _context.Attach<Cart>(cart);
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyCart));
        }
    }
}
