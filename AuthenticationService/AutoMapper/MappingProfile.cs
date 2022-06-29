using AuthenticationService.Database;
using AuthenticationService.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.AutoMapper
{
    /// <summary>
    /// Mapper class
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Two way mapping of objects
            CreateMap<UserModel, User>().ReverseMap();
        }
    }
}
