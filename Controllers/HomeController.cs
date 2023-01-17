using GameChess.Interfaces;
using GameChess.Models;
using GameChess.Models.Games;
using GameChess.Models.GameSettings;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameChess.Controllers
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


        public IActionResult Game(bool mirrorX, string userId)
        {
            IPlayer user = GameCollecton.players.First(e => e.Id == userId);
            return View("~/Views/Game/GameField.cshtml", user);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}