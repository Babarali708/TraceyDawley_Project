using Microsoft.AspNetCore.Mvc;
using TraceyDawley.Helping_Classes;
using TraceyDawley.Models;
using TraceyDawley.Repositories;
using System.Diagnostics;
using System.Drawing;
using static TraceyDawley.Helping_Classes.DtoCollection;

namespace TraceyDawley.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepo userRepo;
        private readonly GeneralPurpose gp;
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor haccess,IUserRepo _userrepo)
        {
            _logger = logger;
            this.userRepo = _userrepo;
            gp = new GeneralPurpose(haccess);
        }
        public async Task<IActionResult> Index(string msg = "", string color = "black")
        {
           // ViewBag.CustomerCount = await userRepo.GetActiveUserCount(3);
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }
        public async Task<IActionResult> addUserForm(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }
        public async Task<IActionResult> addUserFormData(string msg = "", string color = "black")
        {
            User? loggedinUser = gp.GetUserClaims();

            if (loggedinUser==null)
            {
                return RedirectToAction("Login", "Auth", new {msg="Login Again"});
            }

            ViewBag.loggedinUserId=loggedinUser.Id;
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }
        public async Task<IActionResult> PostAddSurveyResponseData(SurveyResponseData _user)
        {
            
            _user.IsActive = 1;
            _user.CreatedAt = GeneralPurpose.DateTimeNow();

            if (!await userRepo.AddSurveyResponseData(_user))
            {
                return RedirectToAction("addUserFormData", "Home", new { msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("ThankYouPage", "Home", new { msg = "Record Inserted Successfully", color = "green" });
        }

        public IActionResult ThankYouPage()
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