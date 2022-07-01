using AutoMapper;
using FlightSearchService.Database;
using MassTransit;
using Shared.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightDetails = Shared.Models.Models.FlightDetails;

namespace FlightSearchService.Consumer
{
    public class FlightDetailsConsumer : IConsumer<FlightDetails>
    {
        private readonly IDataRepository _searchRepos;
        private readonly IMapper _mapper;
        public FlightDetailsConsumer(IDataRepository searchRepos, IMapper mapper)
        {
            _searchRepos = searchRepos;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<FlightDetails> context)
        {
            await Task.Run(() => { var obj = context.Message; });
            //Store to Database
            var flightDetails = context.Message;
            var consumedData = _mapper.Map<FlightDetails, FlightSearchService.Database.FlightDetails>(flightDetails);
            var result = await _searchRepos.SaveFlightDetailsAsync(consumedData).ConfigureAwait(false);
            //Notify the user via Email / SMS
        }
    }
}
