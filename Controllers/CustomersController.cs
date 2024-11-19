using Microsoft.AspNetCore.Mvc;

namespace RestaurantMVC.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
