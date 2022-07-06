using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingService.Database
{
    public class DiscountDetails
    {
        [Key]
        public int DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public double DiscountAmount { get; set; }
    }
}
