using System;

using AutoMapper;
using ISTS.Application.Schedules;
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

        public RoomSessionDto RescheduleSession(Guid sessionId, DateRangeDto newSchedule)
        {
            var entity = _roomRepository.GetSession(sessionId);
            var room = _roomRepository.Get(entity.RoomId);

            var schedule =
                newSchedule == null
                ? null
                : DateRange.Create(newSchedule.Start, newSchedule.End);

            var model = room.RescheduleSession(entity, schedule, _sessionScheduleValidator);
            var updatedEntity = _roomRepository.RescheduleSession(model.Id, model.Schedule);

            var result = _mapper.Map<RoomSessionDto>(updatedEntity);
            return result;
        }

        public RoomSessionDto StartSession(Guid sessionId)
        {
            var entity = _roomRepository.GetSession(sessionId);
            var room = _roomRepository.Get(entity.RoomId);

            var startTime = DateTime.Now;
            var model = room.StartSession(sessionId, startTime);
            _roomRepository.StartSession(sessionId, startTime);

            var result = _mapper.Map<RoomSessionDto>(model);
            return result;
        }

        public RoomSessionDto EndSession(Guid sessionId)
        {
            var entity = _roomRepository.GetSession(sessionId);
            var room = _roomRepository.Get(entity.RoomId);

            var time = DateTime.Now;
            var model = room.EndSession(sessionId, time);
            _roomRepository.EndSession(sessionId, time);

            var result = _mapper.Map<RoomSessionDto>(model);
            return result;
        }
    }
}