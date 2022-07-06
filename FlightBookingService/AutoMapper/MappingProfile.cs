using AutoMapper;
using FlightBookingService.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingService.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Two way mapping of objects
            CreateMap<FlightDetails, Shared.Models.Models.FlightDetails>().ReverseMap();
        }
    }
}
