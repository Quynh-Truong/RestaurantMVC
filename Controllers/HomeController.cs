using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.Models;
using System.Diagnostics;

namespace RestaurantMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7223";

        public HomeController(HttpClient client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            
            ViewData["Title"] = "Home";
            

            return View();
        }



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
