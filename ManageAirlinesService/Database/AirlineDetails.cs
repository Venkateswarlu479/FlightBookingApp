using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManageAirlinesService.Database
{
    public class AirlineDetails
    {
        [Key]
        public long AirlineId { get; set; }
        public string AirlineName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactAddress { get; set; }
        public string AirlineStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastChangedBy { get; set; }
        public DateTime LastChangedDateTime { get; set; }
    }
}
