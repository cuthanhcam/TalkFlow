using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Domain.Aggregates.User;

namespace TalkFlow.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName.Value))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl.Value))
                .ForMember(dest => dest.LastActive, opt => opt.MapFrom(src => src.LastActive))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.IsLocked))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null ? src.Gender.Value : null))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age != null ? (int?)src.Age.Value : null))
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality != null ? src.Nationality.Value : null))
                .ForMember(dest => dest.StrangerFilter, opt => opt.MapFrom(src => src.StrangerFilter));

            CreateMap<StrangerFilter, StrangerFilterDto>()
                .ForMember(dest => dest.FindGender, opt => opt.MapFrom(src => src.FindGender))
                .ForMember(dest => dest.MinAge, opt => opt.MapFrom(src => src.MinAge))
                .ForMember(dest => dest.MaxAge, opt => opt.MapFrom(src => src.MaxAge))
                .ForMember(dest => dest.FindRegion, opt => opt.MapFrom(src => src.FindRegion));
        }

        public static UserDto ToDto(User user)
        {
            return new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                DisplayName = user.DisplayName.Value,
                Email = user.Email ?? string.Empty,
                PhotoUrl = user.PhotoUrl.Value,
                LastActive = user.LastActive,
                IsLocked = user.IsLocked,
                Gender = user.Gender?.Value,
                Age = user.Age?.Value,
                Nationality = user.Nationality?.Value,
                StrangerFilter = user.StrangerFilter != null ? new StrangerFilterDto
                {
                    FindGender = user.StrangerFilter.FindGender,
                    MinAge = user.StrangerFilter.MinAge,
                    MaxAge = user.StrangerFilter.MaxAge,
                    FindRegion = user.StrangerFilter.FindRegion
                } : null
            };
        }
    }
}
