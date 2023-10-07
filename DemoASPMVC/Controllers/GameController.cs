using DemoASPMVC.Models;
using DemoASPMVC_DAL.Interface;
using DemoASPMVC.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DemoASPMVC.Models.ViewModel;

namespace DemoASPMVC.Controllers {
    public class GameController : Controller {

        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;
        private readonly SessionManager _sessionManager;
        public GameController( IGameService gs, IGenreService genreService, SessionManager sessionManager ) {

            _gameService = gs;
            _genreService = genreService;
            _sessionManager = sessionManager;
        }

        public IActionResult Index( int id = 0 ) {

            if( id == 0 )
                return View( _gameService.GetAll( "Game" ).Select( g => g.ToASP() ) );
            return View( _gameService.GetAll( "Game" ).Where( g => g.IdGenre == id ).Select( g => g.ToASP() ) );
        }

        public IActionResult Details( int id ) {

            Game g = _gameService.GetById( "Game", id ).ToASP();
            g.Genre = _genreService.GetById( "Genre", g.IdGenre ).Label;
            return View( g );
        }

        [CustomAuthorize]
        public IActionResult Create() {

            GameForm game = new();
            game.GenreList = _genreService.GetAll( "Genre" );
            return View( game );
        }

        [HttpPost]
        public IActionResult Create( GameForm g ) {

            _gameService.Create( g.ToDal(), _sessionManager.Token );
            return RedirectToAction( "Index" );
        }
        [AdminRequired]
        public IActionResult Delete( int id ) {

            _gameService.Delete( "Game", id, _sessionManager.Token );
            return RedirectToAction( "Index" );
        }

        [CustomAuthorize]
        public IActionResult AddFavorite( int id ) {
            try {

                _gameService.AddFavorite( _sessionManager.ConnectedUser.Id, id, _sessionManager.Token );
                return RedirectToAction( "Index" );
            }
            catch( Exception ex ) {
                TempData["error"] = ex.Message;
                return RedirectToAction( "Index" );
            }
        }

        [CustomAuthorize]
        public IActionResult ViewFavorite() {

            return View( _gameService.GetByUserId( _sessionManager.ConnectedUser.Id, _sessionManager.Token ).Select( x => x.ToASP() ) );
        }
    }
}
