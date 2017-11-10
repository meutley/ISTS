using System;

using ISTS.Application.Common;

namespace ISTS.Application.Rooms
{
    public class RoomFunctionDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public BillingRateDto BaseBillingRate { get; set; }

        public Guid RoomId { get; set; }
    }
}