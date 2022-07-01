using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingService.Database
{
    public class DataRepository : IDataRepository
    {
        private DatabaseContext _dbContext;
        public DataRepository(DatabaseContext context)
        {
            _dbContext = context;
        }

        ///<inheritdoc/>
        public async Task<int> SaveBookingDetails(BookingDetails bookingDetails)
        {
            _dbContext.Entry(bookingDetails).State = (bookingDetails.BookingId == 0) ? EntityState.Added : EntityState.Modified;

            var noOfRowsChanged = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            if (noOfRowsChanged <= 0)
                return 0;

            return bookingDetails.BookingId;
        }

        ///<inheritdoc/>
        public async Task<string> SavePassengerDetails(IEnumerable<PassengerList> passengerList)
        {
            foreach (var passenger in passengerList)
            {
                _dbContext.Entry(passenger).State = (passenger.PassengerId == 0) ? EntityState.Added : EntityState.Modified;
            }

            var noOfRowsChanged = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            if (noOfRowsChanged <= 0)
                return "Error occured while saving passenger data in DB";

            return "Success";
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<BookingDetails>> GetBookingHistoryAsync(string emailId)
        {
            var bookingHistory = await _dbContext.BookingDetails.Where(x => x.Email == emailId).ToListAsync();
            if (!bookingHistory.Any())
                return new List<BookingDetails>();

            return bookingHistory;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<PassengerList>> GetPassengersAsync(int bookingId)
        {
            var passengersInfo = await _dbContext.PassengerList.Where(x => x.BookingId == bookingId).ToListAsync();
            if (!passengersInfo.Any())
                return new List<PassengerList>();

            return passengersInfo;
        }

        ///<inheritdoc/>
        public async Task<string> CancelTicketAsync(string pnrNumber)
        {
            var ticketDetails = await _dbContext.BookingDetails.Where(x => x.PNR == pnrNumber).FirstOrDefaultAsync();
            if (ticketDetails == null)
                return $"No data found for the PNR {pnrNumber}";

            ticketDetails.TicketStatus = "Cancelled";

            _dbContext.Entry(ticketDetails).State = (ticketDetails.BookingId == 0) ? EntityState.Added : EntityState.Modified;
            var noOfRowsChanged = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            if (noOfRowsChanged <= 0)
                return "Error occured while cancelling ticket";

            return "Success";
        }

        ///<inheritdoc/>
        public async Task<BookingDetails> GetTicketDetailsAsync(string pnrNumber)
        {
            var result = from b in _dbContext.BookingDetails.Where(x => x.PNR == pnrNumber)
                         join p in _dbContext.PassengerList on b.BookingId equals p.BookingId
                         select new BookingDetails
                         {
                             BookingId = b.BookingId,
                             Name = b.Name,
                             FlightId = b.FlightId,
                             Email = b.Email,
                             NoOfSeats = b.NoOfSeats,
                             OptForMeal = b.OptForMeal,
                             TicketStatus = b.TicketStatus,
                             TotalPrice = b.TotalPrice,
                             PNR = b.PNR,
                             LastChangedBy = b.LastChangedBy,
                             LastChangedDateTime = b.LastChangedDateTime,
                             CreatedBy = b.CreatedBy,
                             CreatedDateTime = b.CreatedDateTime
                         };
            //var result = await _dbContext.BookingDetails.Where(x => x.PNR == pnrNumber).FirstOrDefaultAsync();
            return await result.FirstOrDefaultAsync();
        }
    }
}
