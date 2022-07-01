using FlightSearchService.Database;
using FlightSearchService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchFlightService.Controllers
{
    /// <summary>
    /// FlightSearchController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class FlightSearchController : ControllerBase
    {
        /// <summary>
        /// _dataRepository
        /// </summary>
        IDataRepository _dataRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataRepository"></param>
        public FlightSearchController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        /// <summary>
        /// Search flight based on input details
        /// </summary>
        /// <param name="flightSearchModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<FlightDetails>>> SearchFlightAsync([FromBody] FlightSearchModel flightSearchModel)
        {
            if (flightSearchModel == null)
                return BadRequest("Invalid Input");
            var result = await _dataRepository.GetFlightDetails(flightSearchModel).ConfigureAwait(false);
            if (result == null || result.Count() == 0)
                return new List<FlightDetails>();

            return Ok(result);
        }
    }
}
