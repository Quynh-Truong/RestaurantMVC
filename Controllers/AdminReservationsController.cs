using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestaurantMVC.Models;
using System.Text;

namespace RestaurantMVC.Controllers
{
    public class AdminReservationsController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7223";
        public AdminReservationsController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {

            var isAdminClaim = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin");

            if (isAdminClaim == null || !bool.TryParse(isAdminClaim.Value, out bool isAdmin) || !isAdmin)
            {
                return Unauthorized("You do not have admin privileges.");
            }

            ViewData["Title"] = "Reservation";


            var response = await _client.GetAsync($"{baseUri}/api/Reservations/getAllReservations");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to list dishes. Please, try again later.";
                return RedirectToAction("Index", "Home");
            }

            var json = await response.Content.ReadAsStringAsync();


            try
            {
                var reservations = JsonConvert.DeserializeObject<List<Reservation>>(json);

                return View(reservations);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unable to list dishes. Please, try again later.";
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Make Reservation";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation reservation)
        {

            var isAdminClaim = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin");

            if (isAdminClaim == null || !bool.TryParse(isAdminClaim.Value, out bool isAdmin) || !isAdmin)
            {
                return Unauthorized("You do not have admin privileges.");
            }

            if (!ModelState.IsValid)
            {
                return View(reservation);
            }

            try
            {
                var json = JsonConvert.SerializeObject(reservation);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await _client.PostAsync($"{baseUri}/api/Reservations/makeReservation", content);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unable to list dishes. Please, try again later.";
                return RedirectToAction("Index");
            }

        }

        public async Task<IActionResult> Edit(int id)
        {

            ViewData["Title"] = "Edit Reservation";

            var response = await _client.GetAsync($"{baseUri}/api/Reservations/getReservation/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to find dish. Please, try again later.";
                return RedirectToAction("Index");
            }

            var json = await response.Content.ReadAsStringAsync();


            try
            {
                var reservation = JsonConvert.DeserializeObject<EditReservationVM>(json);

                return View(reservation);
            }
            catch
            {
                TempData["ErrorMessage"] = "Unable to find dish. Please, try again later.";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditReservationVM reservation)
        {

            var isAdminClaim = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin");

            if (isAdminClaim == null || !bool.TryParse(isAdminClaim.Value, out bool isAdmin) || !isAdmin)
            {
                return Unauthorized("You do not have admin privileges.");
            }

            if (!ModelState.IsValid)
            {
                return View(reservation);
            }
            try
            {
                var json = JsonConvert.SerializeObject(reservation);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await _client.PutAsync($"{baseUri}/api/Reservations/updateReservation/{reservation.ReservationId}", content);
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
            var response = await _client.DeleteAsync($"{baseUri}/api/Reservations/deleteReservation/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to delete dish. Please, try again later.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}

