using AutoMapper;

using ISTS.Application.Common;
using ISTS.Application.Rooms;
using ISTS.Application.Sessions;
using ISTS.Application.Studios;
using ISTS.Application.Users;

using ISTS.Domain.Common;
using ISTS.Domain.Rooms;
using ISTS.Domain.Sessions;
using ISTS.Domain.Studios;
using ISTS.Domain.Users;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DateRange, DateRangeDto>();
        CreateMap<Room, RoomDto>();
        CreateMap<Session, SessionDto>();
        CreateMap<Studio, StudioDto>();
        CreateMap<StudioSearchResult, StudioSearchResultDto>();
        CreateMap<User, UserDto>();
    }
}