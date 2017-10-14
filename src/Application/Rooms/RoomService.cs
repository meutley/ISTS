using System;

using AutoMapper;

using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;

namespace ISTS.Application.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly ISessionScheduleValidator _sessionScheduleValidator;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(
            ISessionScheduleValidator sessionScheduleValidator,
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _sessionScheduleValidator = sessionScheduleValidator;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        
        public RoomSessionDto CreateSession(RoomSessionDto session)
        {
            var room = _roomRepository.Get(session.RoomId);

            var schedule =
                session.Schedule == null
                ? null
                : DateRange.Create(session.Schedule.Start, session.Schedule.End);

            var model = room.CreateSession(schedule, _sessionScheduleValidator);
            var entity = _roomRepository.CreateSession(room.Id, model);

            var result = _mapper.Map<RoomSessionDto>(entity);
            return result;
        }
    }
}