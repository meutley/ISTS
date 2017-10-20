using AutoMapper;

using ISTS.Application.Rooms;
using ISTS.Application.Schedules;
using ISTS.Application.Sessions;
using ISTS.Application.Studios;
using ISTS.Application.Users;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;
using ISTS.Domain.Sessions;
using ISTS.Domain.Studios;
using ISTS.Domain.Users;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DateRange, DateRangeDto>();
        CreateMap<Studio, StudioDto>();
        CreateMap<Room, RoomDto>();
        CreateMap<Session, SessionDto>();
        CreateMap<User, UserDto>();
    }
}