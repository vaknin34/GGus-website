using GGus.Web.Data;
using GGus.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GGus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }



        public IActionResult Index()
        {
            try
            {
                var categories = _context.Category.ToList();

                return View(categories);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        


        public IActionResult SupportUs()
        {

            return View();
        }

        public IActionResult VisitUs()
        {

            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenined()
        {
            return View();
        }

       
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
