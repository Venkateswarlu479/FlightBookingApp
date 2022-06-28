using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManageAirlinesService.Database
{
    public class FlightDetails
    {
        [Key]
        public long FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string AirlineName { get; set; }
        public string FromPlace { get; set; }
        public string ToPlace { get; set; }
        public string DepartureTime { get; set; }
        public string ReachTime { get; set; }
        public string ScheduledDays { get; set; }
        public string InstrumentUsed { get; set; }
        public int NoOfBizClassSeats { get; set; }
        public int NoOfNonBizClassSeats { get; set; }
        public double TicketCost { get; set; }
        public int NoOfRows { get; set; }
        public string OptedForMeal { get; set; }
        public string FlightStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastChangedBy { get; set; }
        public DateTime LastChangedDateTime { get; set; }
    }
}
