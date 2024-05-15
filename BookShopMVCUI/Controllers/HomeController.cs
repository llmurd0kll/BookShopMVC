using BookShopMVCUI.Models;
using BookShopMVCUI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShopMVCUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(int genereId, string sterm ="")
        {
            IEnumerable<Book> books = await _homeRepository.GetBooks(sterm, genereId);
            IEnumerable<Genre> genres = await _homeRepository.Genres();

            BookDisplayModel bookModel = new BookDisplayModel 
            {
                Books = books,
                Genres = genres,
                STerm = sterm,
                GenreId = genereId
            };

            return View(bookModel);
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
    }
}
