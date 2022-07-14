using FlightBookingService.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingService.Models
{
    public class BookingDetailsModel
    {
        public int FlightId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int NoOfSeats { get; set; }
        public string OptForMeal { get; set; }
        public double TotalPrice { get; set; }
        public string AirlineName { get; set; }
        public string BookingClass { get; set; }
        public string[] SeatNumbers { get; set; }
        public List<PassengerListModel> PassengerList { get; set; }
    }
}
