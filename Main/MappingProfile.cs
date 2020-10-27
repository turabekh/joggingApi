using AutoMapper;
using Models.DataTransferObjects.JoggingDtos;
using Models.DataTransferObjects.UserDtos;
using Models.IdentityModels;
using Models.JoggingModels;
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
            CreateMap<User, UserDto>()
                .ForMember(dest =>
                    dest.Joggings,
                    opt => opt.MapFrom(src => src.Joggings)
                    )
                .ReverseMap();
            CreateMap<User, SingleUserDto>()
                    .ForMember(dest =>
                    dest.Joggings,
                    opt => opt.MapFrom(src => src.Joggings)
                    ).ReverseMap();
            CreateMap<UserCreateDto, User>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<Jogging, JoggingDto>().ReverseMap();
            CreateMap<JoggingCreateDto, Jogging>().ReverseMap();
            CreateMap<JoggingUpdateDto, Jogging>().ReverseMap();
        }
    }

}
