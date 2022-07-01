using FlightSearchService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSearchService.Database
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
        public async Task<IEnumerable<FlightDetails>> GetFlightDetails(FlightSearchModel flightSearchModel)
        {
            var result = await _dbContext.FlightDetails.Where(fd => fd.ScheduledDays == flightSearchModel.JourneyDate &&
                                                            fd.FromPlace == flightSearchModel.FromPlace &&
                                                            fd.ToPlace == flightSearchModel.ToPlace &&
                                                            fd.FlightStatus == "Active").ToListAsync();
            return result;
        }

        ///<inheritdoc/>
        public async Task<string> SaveFlightDetailsAsync(FlightDetails flightDetails)
        {
            _dbContext.FlightDetails.Add(flightDetails);
            var noOfRowsChanged = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            if (noOfRowsChanged <= 0)
                return "Internal server error";

            return "Success";
        }
    }
}
