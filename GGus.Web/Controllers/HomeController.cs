using GGus.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GGus.BL.Services;

namespace GGus.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ArticlesRetriever articlesRetriever = new ArticlesRetriever();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            int article = this.articlesRetriever.GetArticleId();
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        public IActionResult Action()
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

   

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
