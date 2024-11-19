using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantMVC.Models;
using System.Text;

namespace RestaurantMVC.Controllers
{
    public class ReservationsController : Controller
    {

        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7223";

        public ReservationsController(HttpClient client)
        {
            _client = client;
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Make Reservation";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if(!ModelState.IsValid)
            {
                return View(reservation);
            }

            try
            {
                var json = JsonConvert.SerializeObject(reservation);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await _client.PostAsync($"{baseUri}/api/Reservations/makeReservation", content);

                return RedirectToAction("Index", "Home");//bekräftelse?
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unable to make reservation. Please, try again later.";
                return RedirectToAction("Index", "Home");
            }
   
        }

        
    }
}
