using FlightSearchService.Models;
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
            var result = _dbContext.FlightDetails.Where(fd => fd.JourneyDate == flightSearchModel.JourneyDate &&
                                                            fd.FromPlace == flightSearchModel.FromPlace &&
                                                            fd.ToPlace == flightSearchModel.ToPlace &&
                                                            fd.TripType == flightSearchModel.TripType &&
                                                            fd.FlightStatus == "Active").ToList();
            return result;
        }
    }
}
