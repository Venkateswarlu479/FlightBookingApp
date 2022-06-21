using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchFlightService.Controllers
{
    [Route("api/v1.0/flight")]
    [ApiController]
    public class FlightSearchController : ControllerBase
    {
        [HttpPost("Search")]
        public string SearchFlight()
        {
            return "No flights found at the moment";
        }
    }
}
