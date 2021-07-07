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

            List<Cart> applicationDbContext = _context.Cart.Include(c => c.User).Include(p => p.Products).ToList();

            foreach (Cart cart in applicationDbContext) {
                cart.User = _context.User.FirstOrDefault(u => u.Id == cart.UserId);
            }
            return View(applicationDbContext);
        }
        [Authorize]
        public IActionResult Search(string query)
        {
            String userName = User.Identity.Name;

            User user = _context.User.FirstOrDefault(x => x.Username == userName);

            Cart cart = _context.Cart.Include(db => db.Products).FirstOrDefault(x => x.UserId == user.Id);



            if(query == null)
                return View("MyCart", cart);

            List<Product> products = cart.Products.Where(p => p.Name.Contains(query) || p.Details.Contains(query)).ToList();
            cart.Products = products;

            return View("MyCart", cart);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchCart(string query)
        {

            List<Cart> carts = _context.Cart.Where(c => c.User.Username.Contains(query)).Include(p => p.Products).ToList();

            foreach (Cart c in carts)
            {
                c.User = _context.User.FirstOrDefault(u => u.Id == c.UserId);
            }

            return PartialView(carts);
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
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }
            cart.User = _context.User.FirstOrDefault(u => u.Id == cart.UserId);
            
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
            cart.User = _context.User.FirstOrDefault(u => u.Id == cart.UserId);
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var cart = await _context.Cart.FindAsync(id);
            String userName = HttpContext.User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            Cart cart = _context.Cart.Include(db => db.Products).FirstOrDefault(x => x.UserId == user.Id);
            cart.Products.Clear();
            cart.TotalPrice = 0;
            _context.Update(cart);

            //_context.Cart.Remove(cart);
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
        [Authorize]
        public async Task<IActionResult> AddToCart(int id) //product id
        {
            Product product = _context.Product.Include(db => db.Carts).FirstOrDefault(x => x.Id == id);
            String userName = HttpContext.User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            Cart cart = _context.Cart.Include(db => db.Products)
             .FirstOrDefault(x => x.UserId == user.Id);


            if (user.Cart.Products == null)
                user.Cart.Products = new List<Product>();
            if (product.Carts == null)
                product.Carts = new List<Cart>();

            if (!(cart.Products.Contains(product) && product.Carts.Contains(cart)))
            {

                user.Cart.Products.Add(product);
                product.Carts.Add(cart);
                user.Cart.TotalPrice += product.Price;
                _context.Update(cart);
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
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
         
            _context.Attach<Cart>(cart);
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyCart));
        }

        [Authorize]
        public async Task<IActionResult> AfterPayment()
        {
            String userName = HttpContext.User.Identity.Name;
            User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
            Cart cart = _context.Cart.Include(db => db.Products).FirstOrDefault(x => x.UserId == user.Id);

            int i = cart.Products.RemoveAll(p => p.Id == p.Id);
            cart.TotalPrice = 0;
            
            _context.Attach<Cart>(cart);
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
