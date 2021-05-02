using GGus.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GGus.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

       


        public IActionResult Adventure()
        {

            return View();
        }


        public IActionResult Twodgames()
        {

            return View();
        }

        public IActionResult Animals()
        {

            return View();
        }


        public IActionResult BoardGames()
        {

            return View();
        }

        public IActionResult Horror()
        {

            return View();
        }


        public IActionResult Music()
        {

            return View();
        }


        public IActionResult Puzzel()
        {

            return View();
        }

        public IActionResult Race()
        {

            return View();
        }


        public IActionResult RPG()
        {

            return View();
        }


        public IActionResult Simulation()
        {

            return View();
        }



        public IActionResult Sports()
        {

            return View();
        }

        public IActionResult Strategy()
        {

            return View();
        }



        public IActionResult BestSeller()
        {

            return View();
        }


        public IActionResult ComingSoon()
        {

            return View();
        }


        public IActionResult GoodDeals()
        {

            return View();
        }



        public IActionResult LastView()
        {

            return View();
        }


        public IActionResult NewGames()
        {

            return View();
        }

        public IActionResult Recommended()
        {

            return View();
        }

        public IActionResult MyGames()
        {

            return View();
        }

        public IActionResult News()
        {

            return View();
        }


        public IActionResult SupportUs()
        {

            return View();
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
