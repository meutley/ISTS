using System;
using System.Threading.Tasks;

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
        
        public async Task<RoomSessionDto> CreateSessionAsync(RoomSessionDto session)
        {
            var room = await _roomRepository.GetAsync(session.RoomId);

            var schedule =
                session.Schedule == null
                ? null
                : DateRange.Create(session.Schedule.Start, session.Schedule.End);

            var model = room.CreateSession(schedule, _sessionScheduleValidator);
            var entity = await _roomRepository.CreateSessionAsync(room.Id, model);

            var result = _mapper.Map<RoomSessionDto>(entity);
            return result;
        }

        public async Task<RoomSessionDto> RescheduleSessionAsync(Guid sessionId, DateRangeDto newSchedule)
        {
            var entity = await _roomRepository.GetSessionAsync(sessionId);
            var room = await _roomRepository.GetAsync(entity.RoomId);

            var schedule =
                newSchedule == null
                ? null
                : DateRange.Create(newSchedule.Start, newSchedule.End);

            var model = room.RescheduleSession(entity, schedule, _sessionScheduleValidator);
            var updatedEntity = await _roomRepository.RescheduleSessionAsync(model.Id, model.Schedule);

            var result = _mapper.Map<RoomSessionDto>(updatedEntity);
            return result;
        }

        public async Task<RoomSessionDto> StartSession(Guid sessionId)
        {
            var entity = await _roomRepository.GetSessionAsync(sessionId);
            var room = await _roomRepository.GetAsync(entity.RoomId);

            var startTime = DateTime.Now;
            var model = room.StartSession(sessionId, startTime);
            await _roomRepository.StartSessionAsync(sessionId, startTime);

            var result = _mapper.Map<RoomSessionDto>(model);
            return result;
        }

        public async Task<RoomSessionDto> EndSessionAsync(Guid sessionId)
        {
            var entity = await _roomRepository.GetSessionAsync(sessionId);
            var room = await _roomRepository.GetAsync(entity.RoomId);

            var time = DateTime.Now;
            var model = room.EndSession(sessionId, time);
            await _roomRepository.EndSessionAsync(sessionId, time);

            var result = _mapper.Map<RoomSessionDto>(model);
            return result;
        }
    }
}