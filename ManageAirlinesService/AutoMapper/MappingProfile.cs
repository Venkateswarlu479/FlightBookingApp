using AutoMapper;
using ManageAirlinesService.Database;
using ManageAirlinesService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageAirlinesService.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Two way Mapping of objects
            CreateMap<AirlineDetailsModel, AirlineDetails>().ReverseMap();
            CreateMap<FlightDetailsModel, FlightDetails>().ReverseMap();
        }
    }
}
