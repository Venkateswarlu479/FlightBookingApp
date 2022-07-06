using AutoMapper;
using ManageAirlinesService.Database;
using ManageAirlinesService.Models;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManageAirlinesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AirlineManagementController : ControllerBase
    {
        private IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public AirlineManagementController(IDataRepository dataRepository, IMapper mapper, IBus bus)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _bus = bus;
        }

        [HttpPost("RegisterAirline")]
        public async Task<ActionResult<string>> RegisterAirlineAsync([FromBody] AirlineDetailsModel airlineRegistrationModel)
        {
            if (airlineRegistrationModel == null)
                return BadRequest("Invalid input");

            var registrationDetails = _mapper.Map<AirlineDetailsModel, AirlineDetails>(airlineRegistrationModel);
            registrationDetails.CreatedBy = airlineRegistrationModel.UserName;
            registrationDetails.CreatedDateTime = DateTime.Now;
            registrationDetails.LastChangedBy = airlineRegistrationModel.UserName;
            registrationDetails.LastChangedDateTime = DateTime.Now;

            var result = await _dataRepository.RegisterAirline(registrationDetails).ConfigureAwait(false);
            if (result == null)
                return "registration unsuccessful";

            return Ok(result);
        }

        [HttpGet("GetAirlines")]
        public async Task<ActionResult<IEnumerable<string>>> GetActiveAirlinesAsync()
        {
            var result = await _dataRepository.GetActiveAirlines().ConfigureAwait(false);
            if (!result.Any() || result == null)
                return new List<string>();
            return Ok(result);
        }

        [HttpPost("BlockAirline")]
        public async Task<ActionResult<string>> BlockAirlineAsync(string airlineName, string userName)
        {
            if (string.IsNullOrWhiteSpace(airlineName))
                return BadRequest("Invalid airlineName");

            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest("Invalid userName");

            var flightDetails = await _dataRepository.UpdateAirlineStatus(airlineName, userName).ConfigureAwait(false);
            if(flightDetails != null && flightDetails.Any())
            {
                foreach (var flightInfo in flightDetails)
                {
                    var publishData = _mapper.Map<FlightDetails, Shared.Models.Models.FlightDetails>(flightInfo);
                    publishData.LastChangedDateTime = DateTime.Now;
                    Uri uri = new Uri("rabbitmq://localhost/flightQueue");
                    var endPoint = await _bus.GetSendEndpoint(uri);
                    await endPoint.Send(publishData);
                }

                return Ok("Success");
            }

            return "Error occured while blocking Airline";
        }

        [HttpPost("AddOrScheduleFlight")]
        public async Task<ActionResult<string>> AddOrScheduleFlightAsync([FromBody] FlightDetailsModel flightDetailsModel)
        {
            if (flightDetailsModel == null)
                return BadRequest("Invalid input");

            var flightDetails = _mapper.Map<FlightDetailsModel, FlightDetails>(flightDetailsModel);
            flightDetails.CreatedBy = flightDetailsModel.CreatedBy;
            flightDetails.CreatedDateTime = DateTime.Now;
            flightDetails.LastChangedBy = flightDetailsModel.CreatedBy;
            flightDetails.LastChangedDateTime = DateTime.Now;
            var result = await _dataRepository.AddOrScheduleFlight(flightDetails).ConfigureAwait(false);

            var publishData = _mapper.Map<FlightDetails, Shared.Models.Models.FlightDetails>(flightDetails);
            if (result != null && publishData != null)
            {
                publishData.LastChangedDateTime = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/flightQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(publishData);
                return Ok(result);
            }

            return "Error Occured while adding/scheduling Flight";
        }

        [HttpGet("FlightDetails/{airlineName}/{flightNumber}/{instrumentUsed}")]
        public async Task<ActionResult<FlightDetailsModel>> GetFlightDetailsAsync(string airlineName, string flightNumber, string instrumentUsed)
        {
            if (string.IsNullOrWhiteSpace(airlineName))
                return BadRequest($"Invalid input: {airlineName}");
            if (string.IsNullOrWhiteSpace(flightNumber))
                return BadRequest($"Invalid input: {flightNumber}");
            if (string.IsNullOrWhiteSpace(instrumentUsed))
                return BadRequest($"Invalid input: {instrumentUsed}");

            var flightDetails = await _dataRepository.GetFlightDetails(airlineName, flightNumber, instrumentUsed).ConfigureAwait(false);
            if (flightDetails == null)
                return Ok(null);
            var result = _mapper.Map<FlightDetails, FlightDetailsModel>(flightDetails);

            return Ok(result);
        }
    }
}
