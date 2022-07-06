using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingService.Database
{
    public class BookingDetails
    {
        [Key]
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int NoOfSeats { get; set; }
        public string OptForMeal { get; set; }
        public ICollection<PassengerList> PassengerList { get; set; }
        public string TicketStatus { get; set; }
        public double TotalPrice { get; set; }
        public string PNR { get; set; }
        public string AirlineName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastChangedBy { get; set; }
        public DateTime LastChangedDateTime { get; set; }
    }
}
