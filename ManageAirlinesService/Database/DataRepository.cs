using ManageAirlinesService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageAirlinesService.Database
{
    /// <summary>
    /// DataRepository class
    /// </summary>
    public class DataRepository : IDataRepository
    {
        /// <summary>
        /// _dbContext
        /// </summary>
        private readonly DatabaseContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public DataRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        ///<inheritdoc/>
        public async Task<string> RegisterAirline(AirlineDetails registrationDetails)
        {
            _dbContext.AirlineDetails.Add(registrationDetails);
            var noOfRowsChanged = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            if (noOfRowsChanged <= 0)
                return "Error occureed while adding Airline details";
            return "Airline Registered Successfully";
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<string>> GetActiveAirlines()
        {
            var airlineDetails = await _dbContext.AirlineDetails.Where(x => x.AirlineStatus == "Active").ToListAsync();
            if (!airlineDetails.Any() || airlineDetails == null)
                return new List<string>();
            return airlineDetails.Select(x => x.AirlineName);
        }

        ///<inheritdoc/>
        public async Task<string> UpdateAirlineStatus(string airlineName, string userName)
        {
            var airlineDetails = await _dbContext.AirlineDetails.Where(x => x.AirlineName == airlineName).FirstOrDefaultAsync();
            if (airlineDetails == null)
                return "No flights found to Block";
            airlineDetails.AirlineStatus = "Inactive";
            airlineDetails.LastChangedBy = userName;
            airlineDetails.LastChangedDateTime = DateTime.Now;
            _dbContext.Entry(airlineDetails).State = (airlineDetails.AirlineId == 0) ? EntityState.Added : EntityState.Modified;

            var flightDetails = await UpdateFlightStatusOfBlockedAirline(airlineName, userName).ConfigureAwait(false);
            foreach (var flightDetail in flightDetails)
            {
                _dbContext.Entry(flightDetail).State = (flightDetail.FlightId == 0) ? EntityState.Added : EntityState.Modified;
            }

            var noOfRowsChanged = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            if (noOfRowsChanged <= 0)
                return "Error occured while updating data in DB";

            return "Airline Blocked successfully";
        }

        /// <summary>
        /// Updating flight status for Blocked airline
        /// </summary>
        /// <param name="airlineName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private async Task<IEnumerable<FlightDetails>> UpdateFlightStatusOfBlockedAirline(string airlineName, string userName)
        {
            var flights = await _dbContext.FlightDetails.Where(x => x.AirlineName == airlineName).ToListAsync();
            foreach (var flight in flights)
            {
                flight.FlightStatus = "Inactive";
                flight.LastChangedBy = userName;
                flight.LastChangedDateTime = DateTime.Now;
            }

            return flights;
        }

        ///<inheritdoc/>
        public async Task<string> AddOrScheduleFlight(FlightDetails flightDetails)
        {
            _dbContext.Entry(flightDetails).State = (flightDetails.FlightId == 0) ? EntityState.Added : EntityState.Modified;
            var noOfRecordsAdded = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            if(noOfRecordsAdded <= 0)
            {
                return "Error occureed while adding inventory or schedule existing airline";
            }
            return "Airline Added/Scheduled Successfully";
        }

        ///<inheritdoc/>
        public async Task<FlightDetails> GetFlightDetails(string airlineName, string flightNumber, string instrumentUsed)
        {
            var flightDetails = await _dbContext.FlightDetails.Where(x => x.AirlineName == airlineName &&
                                                                    x.FlightNumber == flightNumber &&
                                                                    x.InstrumentUsed == instrumentUsed &&
                                                                    x.FlightStatus == "Active").FirstOrDefaultAsync().ConfigureAwait(false);
            if (flightDetails == null)
                return null;
            return flightDetails;
        }
    }
}
