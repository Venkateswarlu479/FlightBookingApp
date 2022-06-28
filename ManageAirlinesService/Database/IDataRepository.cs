using ManageAirlinesService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageAirlinesService.Database
{
    /// <summary>
    /// DataRepository interface
    /// </summary>
    public interface IDataRepository
    {
        /// <summary>
        /// Register airline
        /// </summary>
        /// <param name="registrationDetails"></param>
        /// <returns></returns>
        Task<string> RegisterAirline(AirlineDetails registrationDetails);

        /// <summary>
        /// To get all active airlines
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetActiveAirlines();

        /// <summary>
        /// To Block an airline by updating status as Inactive
        /// </summary>
        /// <param name="airlineName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<string> UpdateAirlineStatus(string airlineName, string userName);

        /// <summary>
        /// add or schedule fight
        /// </summary>
        /// <param name="flightDetails"></param>
        /// <returns></returns>
        Task<string> AddOrScheduleFlight(FlightDetails flightDetails);

        /// <summary>
        /// Gets the flight details based on the search criteria
        /// </summary>
        /// <param name="airlineName"></param>
        /// <param name="flightNumber"></param>
        /// <param name="instrumentUsed"></param>
        /// <returns></returns>
        Task<FlightDetails> GetFlightDetails(string airlineName, string flightNumber, string instrumentUsed);
    }
}
