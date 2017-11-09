using System;

using ISTS.Domain.Common;

namespace ISTS.Domain.Rooms
{
    public class RoomFunction
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public string BillingRateName { get; protected set; }

        public decimal BillingRateUnitPrice { get; protected set; }

        public decimal BillingRateMinimumCharge { get; protected set; }

        public Guid RoomId { get; protected set; }

        public Room Room { get; protected set; }

        public BillingRate BaseBillingRate
        {
            get
            {
                if (string.IsNullOrEmpty(BillingRateName) || BillingRateName == BillingRate.NoneType)
                {
                    return BillingRate.None;
                }
                
                return BillingRateName == BillingRate.HourlyType
                    ? BillingRate.CreateHourly(BillingRateUnitPrice, BillingRateMinimumCharge)
                    : BillingRate.CreateFlatRate(BillingRateUnitPrice);
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.BillingRateName = value.Name;
                this.BillingRateUnitPrice = value.UnitPrice;
                this.BillingRateMinimumCharge = value.MinimumCharge;
            }
        }

        public static RoomFunction Create(
            string name,
            string description,
            Guid roomId)
        {
            Validate(name, description);

            var defaultBillingRate = BillingRate.None;
            return new RoomFunction
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                BaseBillingRate = BillingRate.None,
                RoomId = roomId
            };
        }

        public void SetBillingRate(string name, decimal unitPrice, decimal minimumCharge)
        {
            var billingRate = BillingRate.None;
            switch (name)
            {
                case BillingRate.HourlyType:
                    billingRate = BillingRate.CreateHourly(unitPrice, minimumCharge);
                    break;
                case BillingRate.FlatRateType:
                    billingRate = BillingRate.CreateFlatRate(unitPrice);
                    break;
            }

            ValidateBillingRate(billingRate);
            this.BaseBillingRate = billingRate;
        }

        public decimal CalculateTotalCharge(int hours)
        {
            if (BaseBillingRate == BillingRate.None)
            {
                return 0;
            }

            switch (BaseBillingRate.Name)
            {
                case BillingRate.HourlyType:
                    return CalculateTotalHourly(hours);
                case BillingRate.FlatRateType:
                    return BaseBillingRate.UnitPrice;
                default:
                    return 0;
            }
        }

        private static void Validate(string name, string description)
        {
            if (string.IsNullOrEmpty(name?.Trim()))
            {
                throw new DomainValidationException(new ArgumentException("Name cannot be empty or whitespace"));
            }
        }

        private static void ValidateBillingRate(BillingRate billingRate)
        {
            if (billingRate.UnitPrice < 0)
            {
                throw new DomainValidationException(new ArgumentOutOfRangeException("Unit Price must be greater than or equal to zero"));
            }

            if (billingRate.MinimumCharge < 0)
            {
                throw new DomainValidationException(new ArgumentOutOfRangeException("Minimum Charge must be greater than or equal to zero"));
            }
        }

        private decimal CalculateTotalHourly(int hours)
        {
            var totalCharge = hours * BaseBillingRate.UnitPrice;
            var isTotalLessThanMinimum = totalCharge < BaseBillingRate.MinimumCharge;

            return isTotalLessThanMinimum
                ? BaseBillingRate.MinimumCharge
                : totalCharge;
        }
    }
}