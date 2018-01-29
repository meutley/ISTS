using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using ISTS.Application.Common;
using ISTS.Application.Sessions;
using ISTS.Application.SessionRequests;
using ISTS.Domain.Common;
using ISTS.Domain.Rooms;
using ISTS.Domain.Sessions;
using ISTS.Domain.SessionRequests;

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

        public async Task<RoomDto> GetAsync(Guid id)
        {
            var entity = await _roomRepository.GetAsync(id);

            var result = _mapper.Map<RoomDto>(entity);
            return result;
        }

        public async Task<RoomFunctionDto> AddRoomFunctionAsync(Guid roomId, RoomFunctionDto model)
        {
            var room = await _roomRepository.GetAsync(roomId);
            if (room == null)
            {
                return null;
            }

            var baseBillingRate = model.BaseBillingRate;
            if (baseBillingRate == null)
            {
                baseBillingRate = new BillingRateDto();
            }
            
            var function = room.AddRoomFunction(model.Name, model.Description);
            function.SetBillingRate(
                baseBillingRate.Name,
                baseBillingRate.UnitPrice,
                baseBillingRate.MinimumCharge);
                
            var entity = await _roomRepository.AddRoomFunctionAsync(roomId, function);

            var result = _mapper.Map<RoomFunctionDto>(entity);
            return result;
        }

        public async Task<List<SessionDto>> GetSessionsAsync(Guid roomId)
        {
            var room = await _roomRepository.GetAsync(roomId);
            if (room == null)
            {
                return null;
            }
            
            var sessions = room.Sessions;

            var result = sessions.Select(_mapper.Map<SessionDto>).ToList();
            return result;
        }
        
        public async Task<SessionDto> CreateSessionAsync(Guid roomId, SessionDto session)
        {
            var room = await _roomRepository.GetAsync(roomId);

            var schedule =
                session.Schedule == null
                ? null
                : DateRange.Create(session.Schedule.Start, session.Schedule.End);

            var model = room.CreateSession(schedule, _sessionScheduleValidator);
            var entity = await _roomRepository.CreateSessionAsync(room.Id, model);

            var result = _mapper.Map<SessionDto>(entity);
            return result;
        }

        public async Task<SessionDto> RescheduleSessionAsync(Guid roomId, Guid sessionId, DateRangeDto newSchedule)
        {
            var room = await _roomRepository.GetAsync(roomId);
            var entity = room.Sessions.Single(s => s.Id == sessionId);

            var schedule =
                newSchedule == null
                ? null
                : DateRange.Create(newSchedule.Start, newSchedule.End);

            var model = room.RescheduleSession(entity, schedule, _sessionScheduleValidator);
            var updatedEntity = await _roomRepository.RescheduleSessionAsync(model.Id, model.Schedule);

            var result = _mapper.Map<SessionDto>(updatedEntity);
            return result;
        }

        public async Task<SessionDto> StartSessionAsync(Guid roomId, Guid sessionId)
        {
            var room = await _roomRepository.GetAsync(roomId);
            var entity = room.Sessions.Single(s => s.Id == sessionId);

            var startTime = DateTime.Now;
            var model = room.StartSession(sessionId, startTime);
            await _roomRepository.StartSessionAsync(sessionId, startTime);

            var result = _mapper.Map<SessionDto>(model);
            return result;
        }

        public async Task<SessionDto> EndSessionAsync(Guid roomId, Guid sessionId)
        {
            var room = await _roomRepository.GetAsync(roomId);
            var entity = room.Sessions.Single(s => s.Id == sessionId);

            var time = DateTime.Now;
            var model = room.EndSession(sessionId, time);
            await _roomRepository.EndSessionAsync(sessionId, time);

            var result = _mapper.Map<SessionDto>(model);
            return result;
        }

        public async Task<SessionRequestDto> RequestSessionAsync(SessionRequestDto model)
        {
            var requestedDateRange = model.RequestedTime != null
                ? DateRange.Create(model.RequestedTime.Start, model.RequestedTime.End)
                : null;
            
            var room = await _roomRepository.GetAsync(model.RoomId);
            var newModel = room.RequestSession(model.RequestingUserId, requestedDateRange, model.RoomFunctionId, _sessionScheduleValidator);
            var entity = await _roomRepository.RequestSessionAsync(newModel);

            var result = _mapper.Map<SessionRequestDto>(entity);
            return result;
        }
        public async Task<SessionRequestDto> ApproveSessionRequestAsync(Guid roomId, Guid requestId)
        {
            // Create the request model first
            var room = await _roomRepository.GetAsync(roomId);
            var requestModel = room.ApproveSessionRequest(requestId, _sessionScheduleValidator);

            // Create the new Session from the approved request, then link the request to the session and persist changes
            var newSession = room.CreateSession(requestModel.RequestedTime, requestModel.Id, _sessionScheduleValidator);
            requestModel.LinkToSession(newSession.Id);
            await _roomRepository.ApproveSessionRequestAsync(requestModel);

            var result = _mapper.Map<SessionRequestDto>(requestModel);
            return result;
        }

        public async Task<SessionRequestDto> RejectSessionRequestAsync(Guid roomId, Guid requestId, string reason)
        {
            var room = await _roomRepository.GetAsync(roomId);
            var model = room.RejectSessionRequest(requestId, reason);
            await _roomRepository.RejectSessionRequestAsync(model);

            var result = _mapper.Map<SessionRequestDto>(model);
            return result;
        }
    }
}