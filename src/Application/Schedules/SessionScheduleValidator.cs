using System;
using System.Collections.Generic;
using System.Linq;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;

namespace ISTS.Application.Schedules
{
    public class SessionScheduleValidator : ISessionScheduleValidator
    {
        private readonly IRoomRepository _roomRepository;

        public SessionScheduleValidator(
            IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public SessionScheduleValidatorResult Validate(Guid roomId, Guid? sessionId, DateRange schedule)
        {
            if (schedule != null)
            {
                if (schedule.Start >= schedule.End)
                {
                    throw new ScheduleEndMustBeGreaterThanStartException();
                }

                var roomSchedule =
                    _roomRepository
                    .GetSchedule(roomId, schedule)
                    .Where(s => s.SessionId != sessionId)
                    .ToList();

                if (roomSchedule.Any() && DoesScheduleOverlap(schedule, roomSchedule))
                {
                    throw new OverlappingScheduleException();
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