using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ISTS.Domain.Rooms;
using ISTS.Domain.Common;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Sessions
{
    public class SessionScheduleValidator : ISessionScheduleValidator
    {
        private readonly IRoomRepository _roomRepository;

        public SessionScheduleValidator(
            IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<SessionScheduleValidatorResult> ValidateAsync(Guid roomId, Guid? sessionId, DateRange schedule)
        {
            if (schedule != null)
            {
                if (schedule.Start >= schedule.End)
                {
                    throw new DomainValidationException(new ScheduleEndMustBeGreaterThanStartException());
                }

                var roomScheduleEntities = await _roomRepository.GetScheduleAsync(roomId, schedule);
                var roomSchedule =
                    roomScheduleEntities
                    .Where(s => s.SessionId != sessionId)
                    .ToList();

                if (roomSchedule.Any() && DoesScheduleOverlap(schedule, roomSchedule))
                {
                    throw new DomainValidationException(new OverlappingScheduleException());
                }
            }

            return SessionScheduleValidatorResult.Success;
        }

        private bool DoesScheduleOverlap(DateRange schedule, List<RoomSessionSchedule> roomSchedule)
        {
            var overlap =
                roomSchedule
                .Where(s => s.Schedule != null && DoDateRangesOverlap(schedule, s.Schedule));

            return overlap.Any();
        }

        private bool DoDateRangesOverlap(DateRange left, DateRange right)
        {
            return
                (left.Start >= right.Start && left.Start < right.End)
                || (left.End >= right.Start && left.End < right.End)
                || (left.Start >= right.Start && left.End <= right.End);
        }
    }
}