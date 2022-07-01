using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageAirlinesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightDataController : ControllerBase
    {
        private readonly IBus _bus;
        public FlightDataController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<ActionResult<string>> PublishFlightDataAsync(FlightDetails flightDetails)
        {
            if(flightDetails != null)
            {
                flightDetails.LastChangedDateTime = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/flightQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(flightDetails);
                return Ok("Successfully published data");
            }

            return BadRequest("invalid data");
        }
    }
}
