using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.Room;
using TalkFlow.Domain.Aggregates.Room;

namespace TalkFlow.Application.Mappings
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.HostId, opt => opt.MapFrom(src => src.HostId))
                .ForMember(dest => dest.HostDisplayName, opt => opt.MapFrom(src => "Host")) // populated from user data
                .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Capacity.Value))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.IsChatBlocked, opt => opt.MapFrom(src => src.IsChatBlocked))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.SecurityCode, opt => opt.MapFrom(src => src.SecurityCode.IsEmpty ? null : src.SecurityCode.Value));
        }

        public static RoomDto ToDto(Room room)
        {
            return new RoomDto
            {
                RoomId = room.Id,
                RoomName = room.Name.Value,
                HostId = room.HostId,
                HostDisplayName = "Host", // populated from user data
                MemberCount = room.Capacity.Value,
                CreatedAt = room.CreatedAt,
                IsChatBlocked = room.IsChatBlocked,
                IsActive = room.IsActive,
                SecurityCode = room.SecurityCode.IsEmpty ? null : room.SecurityCode.Value
            };
        }
    }
}
