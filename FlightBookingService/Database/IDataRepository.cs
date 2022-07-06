using FlightBookingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingService.Database
{
    public interface IDataRepository
    {
        /// <summary>
        /// Gets the available flight details
        /// </summary>
        /// <param name="flightSearchModel"></param>
        /// <returns></returns>
        Task<IEnumerable<FlightDetails>> GetFlightDetails(FlightSearchModel flightSearchModel);

        /// <summary>
        /// To save flight details in DB for search operation
        /// </summary>
        /// <param name="flightDetails"></param>
        /// <returns></returns>
        Task<string> SaveFlightDetailsAsync(FlightDetails flightDetails);

        /// <summary>
        /// To save booking details in database
        /// </summary>
        /// <param name="bookingDetails"></param>
        /// <param name="passengerList"></param>
        /// <returns></returns>
        Task<int> SaveBookingDetails(BookingDetails bookingDetails);

        /// <summary>
        /// To save passenger details in DB
        /// </summary>
        /// <param name="passengerList"></param>
        /// <returns></returns>
        Task<string> SavePassengerDetails(IEnumerable<PassengerList> passengerList);

        /// <summary>
        /// To get booking history of particular user by emailId
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingDetails>> GetBookingHistoryAsync(string emailId);

        /// <summary>
        /// To get passengers info of a specific booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<IEnumerable<PassengerList>> GetPassengersAsync(int bookingId);

        /// <summary>
        /// To cancel booked ticket prior to 24 hrs by pnrNumber
        /// </summary>
        /// <param name="pnrNumber"></param>
        /// <returns></returns>
        Task<string> CancelTicketAsync(string pnrNumber);

        /// <summary>
        /// To get booked ticket details by PNR number
        /// </summary>
        /// <param name="pnrNumber"></param>
        /// <returns></returns>
        Task<BookingDetails> GetTicketDetailsAsync(string pnrNumber);

        /// <summary>
        /// to apply discount on ticket price
        /// </summary>
        /// <param name="discountCode"></param>
        /// <returns></returns>
        Task<DiscountDetails> GetDiscountDetailsAsync(string discountCode);

        /// <summary>
        /// Add Discount details
        /// </summary>
        /// <param name="discount"></param>
        /// <returns></returns>
        Task<string> AddDiscountCodeAsync(DiscountDetails discount);
    }
}
