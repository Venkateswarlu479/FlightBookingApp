using FlightSearchService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSearchService.Database
{
    /// <summary>
    /// DataRepository interface
    /// </summary>
    public interface IDataRepository
    {
        /// <summary>
        /// Gets the available flight details
        /// </summary>
        /// <param name="flightSearchModel"></param>
        /// <returns></returns>
        Task<IEnumerable<FlightDetails>> GetFlightDetails(FlightSearchModel flightSearchModel);
    }
}
