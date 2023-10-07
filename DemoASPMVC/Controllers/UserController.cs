using DemoASPMVC.Models.ViewModel;
using DemoASPMVC.Tools;
using DemoASPMVC_DAL.Interface;
using DemoASPMVC_DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DemoASPMVC.Controllers
{
    public class UserController : Controller
    {
        
        private readonly IUserService _userService;
        private readonly SessionManager _session;

        public UserController(IUserService userService, SessionManager session)
        {
            _userService = userService;
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterForm u)
        {
            if (!ModelState.IsValid)
                return View(u);
            

            if (_userService.Register(u.Email, u.Password, u.Nickname))
                return RedirectToAction("Index", "Game");
            
            return View();

        }

        public IActionResult Login()
        {
            ViewData["toto"] = DateTime.Now;
            TempData["bidule"] = "jean maurice";
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginForm u)
        {
            try
            {
                UserLogin connectedUser = _userService.Login(u.Email, u.Password);
                _session.ConnectedUser = connectedUser.User;
                _session.Token = connectedUser.Token;
                return RedirectToAction("Index", "Game");

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            _session.Logout();
            return RedirectToAction("Index", "Game");
        }

        public IActionResult GetById(int id)
        {
            return Ok(_userService.GetById("Users", id));
        }
    }
}
