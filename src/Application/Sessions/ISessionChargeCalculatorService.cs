using System;
using System.Threading.Tasks;

using ISTS.Domain.Rooms;

namespace ISTS.Application.Sessions
{
    public interface ISessionChargeCalculatorService
    {
        Task<decimal> CalculateTotalCharge(Guid sessionId);
    }
}