using System.ComponentModel.DataAnnotations;

namespace RestaurantMVC.Models
{
    public class EditReservationVM
    {
        public int ReservationId { get; set; }
        //public string FirstName { get; set; } 
        //public string LastName { get; set; }  
        //public string PhoneNo { get; set; }
        [Required]
        public int TableId { get; set; }
        [Required]
        public DateTime ReservationStart { get; set; }

        [Required(ErrorMessage = "Number of people is required.")]
        [Range(1, 8, ErrorMessage = "Number must be between 1 and 8.")]
        public int NoOfPeople { get; set; }
    }
}
