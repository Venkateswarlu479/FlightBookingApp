using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingService.Database
{
    public class PassengerList
    {
        [Key]
        public int PassengerId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string SeatNo { get; set; }
        public BookingDetails BookingDetails { get; set; }
        public int BookingId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastChangedBy { get; set; }
        public DateTime LastChangedDateTime { get; set; }

    }
}
