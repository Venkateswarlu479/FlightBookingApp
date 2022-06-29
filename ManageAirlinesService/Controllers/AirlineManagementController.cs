using AutoMapper;
using ManageAirlinesService.Database;
using ManageAirlinesService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public AirlineManagementController(IDataRepository dataRepository, IMapper mapper)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
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

            var result = await _dataRepository.UpdateAirlineStatus(airlineName, userName).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("AddOrScheduleFlight")]
        public async Task<ActionResult<string>> AddOrScheduleFlightAsync([FromBody] FlightDetailsModel flightDetailsModel)
        {
            if (flightDetailsModel == null)
                return BadRequest("Invalid input");
            var flightDetails = _mapper.Map<FlightDetailsModel, FlightDetails>(flightDetailsModel);
            var result = await _dataRepository.AddOrScheduleFlight(flightDetails).ConfigureAwait(false);
            if (result == null)
                return "Error Occured while adding/scheduling Flight";

            return Ok(result);
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
