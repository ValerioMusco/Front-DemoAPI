using DemoASPMVC.Models;
using DemoASPMVC.Tools;
using DemoASPMVC_DAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DemoASPMVC.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly SessionManager _sessionManager;
        public GenreController(IGenreService genreService, SessionManager session)
        {
            _genreService = genreService;
            _sessionManager = session;
        }
        public IActionResult Index()
        {
            return View(_genreService.GetAll("Genre"));
        }

        [AdminRequired]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GenreForm f)
        {
            _genreService.Add( f.Label, _sessionManager.Token );
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return RedirectToAction("Index","Game", new { id });
        }
    }
}
