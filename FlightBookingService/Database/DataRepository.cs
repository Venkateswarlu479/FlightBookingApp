using FlightBookingService.Models;
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
        public async Task<IEnumerable<FlightDetails>> GetFlightDetails(FlightSearchModel flightSearchModel)
        {
            var result = await _dbContext.FlightDetails.Where(fd => fd.ScheduledDate == flightSearchModel.JourneyDate &&
                                                            fd.FromPlace == flightSearchModel.FromPlace &&
                                                            fd.ToPlace == flightSearchModel.ToPlace &&
                                                            fd.FlightStatus == "Active").ToListAsync();
            return result;
        }

        ///<inheritdoc/>
        public async Task<string> SaveFlightDetailsAsync(FlightDetails flightDetails)
        {
            try
            {
                var flightDetailsExist = await _dbContext.FlightDetails.Where(x => x.FlightId == flightDetails.FlightId && x.FlightStatus == "Active")
                                                                        .AsNoTracking().FirstOrDefaultAsync();
                if (flightDetailsExist == null)
                {
                    await _dbContext.FlightDetails.AddAsync(flightDetails);
                }
                else
                {
                    flightDetails.SearchSeqNo = flightDetailsExist.SearchSeqNo;
                    _dbContext.Entry(flightDetails).State = EntityState.Modified;

                    //cancel tickets if any booked with the flights we are blocking
                    var cancellationStatus = await CancelTicketsOfBlockedFlightAsync(flightDetails.FlightId).ConfigureAwait(false);
                    if (cancellationStatus == null || cancellationStatus == "Internal Server Error")
                        return "Internal server error";

                }
                var noOfRowsChanged = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                if (noOfRowsChanged <= 0)
                    return "Internal server error";

                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<string> CancelTicketsOfBlockedFlightAsync(long flightId)
        {
            var bookingDetails = await _dbContext.BookingDetails.Where(x => x.FlightId == flightId && x.TicketStatus != "Cancelled").AsNoTracking().ToListAsync();
            if (!bookingDetails.Any())
                return "No tickets active with the blocked flight";
            foreach (var bookingInfo in bookingDetails)
            {
                bookingInfo.TicketStatus = "Cancelled";
                _dbContext.Entry(bookingInfo).State = EntityState.Modified;
            }
            var noOfRowsChanged = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            if (noOfRowsChanged <= 0)
                return "Internal Server Error";

            return "Success";
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
            //var bookingHistory = await _dbContext.BookingDetails.Where(x => x.Email == emailId).ToListAsync();
            //if (!bookingHistory.Any())
            //    return new List<BookingDetails>();

            //return bookingHistory;
            var result = from b in _dbContext.BookingDetails.Where(x => x.Email == emailId)
                         join p in _dbContext.PassengerList on b.BookingId equals p.BookingId
                         select new BookingDetails()
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
                             CreatedDateTime = b.CreatedDateTime,
                             AirlineName = b.AirlineName,
                             PassengerList = b.PassengerList
                         };

            return await result.ToListAsync().ConfigureAwait(false);
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
                             CreatedDateTime = b.CreatedDateTime,
                             AirlineName = b.AirlineName,
                             PassengerList = b.PassengerList
                         };

            return await result.FirstOrDefaultAsync();
        }

        ///<inheritdoc/>
        public async Task<DiscountDetails> GetDiscountDetailsAsync(string discountCode)
        {
            var result = await _dbContext.DiscountDetails.Where(x => x.DiscountCode == discountCode).FirstOrDefaultAsync();
            return result;
        }

        ///<inheritdoc/>
        public async Task<string> AddDiscountCodeAsync(DiscountDetails discount)
        {
            await _dbContext.DiscountDetails.AddAsync(discount).ConfigureAwait(false);
            var noOfRowsChanged = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            if (noOfRowsChanged <= 0)
                return "Error occured while adding Discount data";
            return "Discount data added Successfully";
        }
    }
}
