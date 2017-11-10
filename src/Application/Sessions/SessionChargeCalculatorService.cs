using System;
using System.Linq;
using System.Threading.Tasks;

using ISTS.Application.Common;
using ISTS.Domain.Common;
using ISTS.Domain.Rooms;
using ISTS.Domain.Sessions;

namespace ISTS.Application.Sessions
{
    public class SessionChargeCalculatorService : ISessionChargeCalculatorService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IRoomRepository _roomRepository;

        public SessionChargeCalculatorService(
            ISessionRepository sessionRepository,
            IRoomRepository roomRepository)
        {
            _sessionRepository = sessionRepository;
            _roomRepository = roomRepository;
        }
        
        public async Task<decimal> CalculateTotalCharge(Guid sessionId)
        {
            var session = await _sessionRepository.GetAsync(sessionId);
            if (!session.RoomFunctionId.HasValue)
            {
                return 0;
            }
            
            // Make sure the session has started and ended
            if (!session.ActualStartTime.HasValue)
            {
                throw new DataValidationException(new SessionNotStartedException("The session has not been started yet"));
            }

            if (!session.ActualEndTime.HasValue)
            {
                throw new DataValidationException(new SessionNotEndedException());
            }
            
            var room = await _roomRepository.GetAsync(session.RoomId);
            var function = room.RoomFunctions.Single(x => x.Id == session.RoomFunctionId);

            // Calculate and return the total based on the room function billing rate and the total billable time
            var baseBillingRate = function.BaseBillingRate;
            var billableTime = session.ActualEndTime.Value - session.ActualStartTime.Value;
            decimal totalCharge = 0;
            switch (baseBillingRate?.Name)
            {
                case BillingRate.HourlyType:
                    totalCharge = CalculateTotalHourlyCharge(baseBillingRate.UnitPrice, baseBillingRate.MinimumCharge, billableTime);
                    break;
                case BillingRate.FlatRateType:
                    totalCharge = baseBillingRate.UnitPrice;
                    break;
                default:
                    totalCharge = 0;
                    break;
            }

            return totalCharge;
        }

        private decimal CalculateTotalHourlyCharge(decimal unitPrice, decimal minimumCharge, TimeSpan time)
        {
            decimal total = unitPrice * time.Hours;
            var isTotalLessThanMinimum = total < minimumCharge;

            return
                isTotalLessThanMinimum
                ? minimumCharge
                : total;
        }
    }
}