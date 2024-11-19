using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantMVC.Models
{
    public class Reservation
    {
   
        public int ReservationId { get; set; }
        //public int CustomerId { get; set; }
        //[Required(ErrorMessage = "First name is required.")]
        //[StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }
        //[Required(ErrorMessage = "Last name is required.")]
        //[StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; set; }
        //[Required(ErrorMessage = " Phone no is required.")]
        //[Phone(ErrorMessage = "Invalid phone number.")]

        public string PhoneNo { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        [Required(ErrorMessage = "Number of people is required.")]
        [Range(1, 8, ErrorMessage = "Number must be between 1-8.")]
        public int NoOfPeople { get; set; }
    }
}
