using Microsoft.AspNetCore.Mvc;
using ProjectHub.ViewModels;
using System.Diagnostics;

namespace ProjectHub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Mail was send successfully!";
                ModelState.Clear();
            }

            return View(new ContactViewModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
