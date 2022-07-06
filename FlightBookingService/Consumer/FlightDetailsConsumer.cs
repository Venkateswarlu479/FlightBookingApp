using AutoMapper;
using FlightBookingService.Database;
using MassTransit;
using Shared.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightDetails = Shared.Models.Models.FlightDetails;

namespace FlightBookingService.Consumer
{
    public class FlightDetailsConsumer : IConsumer<FlightDetails>
    {
        private readonly IDataRepository _bookingRepos;
        private readonly IMapper _mapper;
        public FlightDetailsConsumer(IDataRepository bookingRepos, IMapper mapper)
        {
            _bookingRepos = bookingRepos;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<FlightDetails> context)
        {
            await Task.Run(() => { var obj = context.Message; });
            //Store to Database
            var flightDetails = context.Message;
            var consumedData = _mapper.Map<FlightDetails, Database.FlightDetails>(flightDetails);
            var result = await _bookingRepos.SaveFlightDetailsAsync(consumedData).ConfigureAwait(false);
        }
    }
}
