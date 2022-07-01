using AutoMapper;
using FlightSearchService.Database;
using FlightSearchService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSearchService.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Two way Mapping of objects
            CreateMap<FlightDetails, Shared.Models.Models.FlightDetails>().ReverseMap();
        }
    }
}
