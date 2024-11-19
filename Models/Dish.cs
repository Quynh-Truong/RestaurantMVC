using System.ComponentModel.DataAnnotations;

namespace RestaurantMVC.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool Availability { get; set; }
        public bool IsPopular { get; set; }

        public string ImageUrl { get; set; }
    }
}
