using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestaurantMVC.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace RestaurantMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7223";
        public MenuController(HttpClient client)
        {
            _client = client;
        }

        //has a hidden GET
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Menu";


            var response = await _client.GetAsync($"{baseUri}/api/Menu/getAllDishes");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to list dishes. Please, try again later.";
                return RedirectToAction("Index", "Home");
            }

            var json = await response.Content.ReadAsStringAsync();//turn array of json into string

            try
            {
                //what object do we want to match => list of dishes with id, name...
                var menu = JsonConvert.DeserializeObject<List<Dish>>(json);

                return View(menu);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unable to list dishes. Please, try again later.";
                return RedirectToAction("Index", "Home");
            }

        }
  
    }
}
