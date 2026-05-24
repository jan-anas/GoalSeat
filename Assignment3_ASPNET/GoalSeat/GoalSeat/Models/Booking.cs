using System.ComponentModel.DataAnnotations;

namespace GoalSeat.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Customer Email")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Match Name")]
        public string MatchName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Ticket Type")]
        public string TicketType { get; set; } = string.Empty;

        [Range(1, 10)]
        [Display(Name = "Number of Tickets")]
        public int TicketCount { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
    }
}