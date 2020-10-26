using AutoMapper;
using Models.DataTransferObjects.UserDtos;
using Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterationDto, User>();
        }
    }

}
