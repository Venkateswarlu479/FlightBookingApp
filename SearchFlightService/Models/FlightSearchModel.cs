using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSearchService.Models
{
    /// <summary>
    /// Flight Search Model
    /// </summary>
    public class FlightSearchModel
    {
        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public string TripType { get; set; }

        public string JourneyDate { get; set; }
    }
}
