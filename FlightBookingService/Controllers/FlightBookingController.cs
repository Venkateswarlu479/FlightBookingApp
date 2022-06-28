using AutoMapper;
using FlightBookingService.Database;
using FlightBookingService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightBookingController : ControllerBase
    {
        private readonly IDataRepository _repository;
        private readonly IMapper _mapper;
        public FlightBookingController(IDataRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("BookTicket")]
        public async Task<ActionResult<string>> BookFlightTicketAsync([FromBody] BookingDetailsModel bookingDetails)
        {
            if (bookingDetails == null)
            {
                return BadRequest("Invalid input data");
            }

            var pnrNumber = GenerateRandomPNRNumber();

            var bookingInfo = new BookingDetails()
            {
                FlightId = bookingDetails.FlightId,
                Name = bookingDetails.Name,
                Email = bookingDetails.Email,
                NoOfSeats = bookingDetails.NoOfSeats,
                OptForMeal = bookingDetails.OptForMeal,
                TotalPrice = bookingDetails.TotalPrice,
                TicketStatus = "Booked",
                PNR = pnrNumber,
                CreatedBy = bookingDetails.Name,
                CreatedDateTime = DateTime.Now,
                LastChangedBy = bookingDetails.Name,
                LastChangedDateTime = DateTime.Now
            };

            var bookingResult = await _repository.SaveBookingDetails(bookingInfo).ConfigureAwait(false);
            if (bookingResult <= 0)
                return "Invalid result returned due to getting error while saving Booking details in DB";

            var bookingId = bookingResult;
            var passengersInfo = new List<PassengerList>();
            for(int i=0; i < bookingDetails.PassengerList.Count(); i++)
            {
                var passengerDetails = new PassengerList()
                {
                    Name = bookingDetails.PassengerList[i].Name,
                    Age = bookingDetails.PassengerList[i].Age,
                    Gender = bookingDetails.PassengerList[i].Gender,
                    SeatNo = bookingDetails.SeatNumbers[i],
                    BookingId = bookingId,
                    CreatedBy = bookingDetails.Name,
                    CreatedDateTime = DateTime.Now,
                    LastChangedBy = bookingDetails.Name,
                    LastChangedDateTime = DateTime.Now
                };
                passengersInfo.Add(passengerDetails);
            }
            var passengerResult = await _repository.SavePassengerDetails(passengersInfo).ConfigureAwait(false);
            if (passengerResult == null)
                return "Invalid result returned due to getting error while saving Passenger details in DB";

            return Ok(pnrNumber);
        }

        [HttpPost("CancelTicket")]
        public async Task<ActionResult<string>> CancelTicketAsync(string pnrNumber)
        {
            if (string.IsNullOrWhiteSpace(pnrNumber))
                return BadRequest("Invalid input");

            var cancellationStatus = await _repository.CancelTicketAsync(pnrNumber).ConfigureAwait(false);
            if (cancellationStatus == null)
                return "Cancellation unsuccessful due to error";

            return Ok("Cancelled Successfully");
        }

        [HttpGet("BookingHistory/{emailId}")]
        public async Task<ActionResult<IEnumerable<BookingDetails>>> GetBookingHistoryAsync(string emailId)
        {
            if (string.IsNullOrWhiteSpace(emailId))
                return BadRequest("Invalid input");
            var bookingHistory = await _repository.GetBookingHistoryAsync(emailId).ConfigureAwait(false);
            if (!bookingHistory.Any())
                return new List<BookingDetails>();

            return Ok(bookingHistory);
        }

        [HttpGet("PassengersData/{bookingId}")]
        public async Task<ActionResult<IEnumerable<PassengerList>>> GetPassengersDataAsync(int bookingId)
        {
            if (bookingId <= 0)
                return BadRequest("Invalid input");
            var passengersInfo = await _repository.GetPassengersAsync(bookingId).ConfigureAwait(false);
            if (!passengersInfo.Any())
                return new List<PassengerList>();

            return Ok(passengersInfo);
        }

        [HttpGet("TicketDetails/{pnrNumber}")]
        public async Task<ActionResult<BookingDetails>> GetBookedTicketDetails(string pnrNumber)
        {
            if (string.IsNullOrWhiteSpace(pnrNumber))
                return BadRequest("Invalid input");
            var ticketDetails = await _repository.GetTicketDetailsAsync(pnrNumber).ConfigureAwait(false);
            if (ticketDetails == null)
                return new BookingDetails();

            return Ok(ticketDetails);
        }

        /// <summary>
        /// Generating alpha numeric PNR number
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomPNRNumber()
        {
            const string src = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int length = 6;
            var sb = new StringBuilder();
            Random random = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[random.Next(0, src.Length)];
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
