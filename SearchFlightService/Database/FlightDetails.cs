using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSearchService.Database
{
    public class FlightDetails
    {
        [Key]
        public long FlightId { get; set; }

        public string FlightNumber { get; set; }

        public string FlightName { get; set; }

        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public string TripType { get; set; }

        public string JourneyDate { get; set; }

        public double TicketPrice { get; set; }

        public string FlightStatus { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string LastChangedBy { get; set; }

        public DateTime LastChangedDateTime { get; set; }
    }
}
    