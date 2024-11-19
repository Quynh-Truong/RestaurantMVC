using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantMVC.Models;
using System.Net.Http.Headers;
using System.Text;

namespace RestaurantMVC.Controllers
{
    public class AdminMenuController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7223";
        public AdminMenuController(HttpClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index()
        {
            //checks that admin is authorized
            var isAdminClaim = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin");

            if (isAdminClaim == null || !bool.TryParse(isAdminClaim.Value, out bool isAdmin) || !isAdmin)
            {
                return Unauthorized("You do not have admin privileges.");
            }

            ViewData["Title"] = "AdminMenu";


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

        public IActionResult Create()//this and the method below should match, since they are a pair
                                     //first leads to view, the other makes the post
        {
            ViewData["Title"] = "New Dish";

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Dish dish)
        {

            var isAdminClaim = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin");

            if (isAdminClaim == null || !bool.TryParse(isAdminClaim.Value, out bool isAdmin) || !isAdmin)
            {
                return Unauthorized("You do not have admin privileges.");
            }

            if (!ModelState.IsValid)
            {
                return View(dish);
            }

            try
            {
                var json = JsonConvert.SerializeObject(dish); //convert object to json-string

                //attach json-string to the body being sent, declaring that it's json
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //request and body is sent
                /*var response = */
                await _client.PostAsync($"{baseUri}/api/Menu/addDish", content);

                return RedirectToAction("Index");//return to index - with all dishes
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unable to create dish. Please, try again later.";
                return RedirectToAction("Index");
            }


        }

        public async Task<IActionResult> Edit(int id)
        {

            var response = await _client.GetAsync($"{baseUri}/api/Menu/getDish/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to find dish to update.";
                return RedirectToAction("Index");
            }

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var dish = JsonConvert.DeserializeObject<Dish>(json);

                return View(dish);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unable to find dish to update.";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Dish dish)
        {
            var isAdminClaim = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin");

            if (isAdminClaim == null || !bool.TryParse(isAdminClaim.Value, out bool isAdmin) || !isAdmin)
            {
                return Unauthorized("You do not have admin privileges.");
            }

            if (!ModelState.IsValid)/*fixa felmeddel i klass*/
            {
                return View(dish);
            }

            try
            {
                var json = JsonConvert.SerializeObject(dish);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                /*var response =*/
                await _client.PutAsync($"{baseUri}/api/Menu/updateDish/{dish.DishId}", content);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unable to update dish. Please, try again later.";
                return RedirectToAction("Index");

            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{baseUri}/api/Menu/deleteDish/{id}");


            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to delete dish. Please, try again later.";
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");

        }
    }
}
