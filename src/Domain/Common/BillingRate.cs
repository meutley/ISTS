using System;
using System.Collections.Generic;
using System.Linq;

namespace ISTS.Domain.Common
{
    public class BillingRate : ValueObject<BillingRate>
    {
        public const string NoneType = "None";
        public const string HourlyType = "Hourly";
        public const string FlatRateType = "FlatRate";

        public static BillingRate None =>
            new BillingRate
            {
                Name = NoneType,
                UnitPrice = 0,
                MinimumCharge = 0
            };
        
        public string Name { get; protected set; }

        public decimal UnitPrice { get; protected set; }

        public decimal MinimumCharge { get; protected set; }

        public static BillingRate CreateHourly(decimal unitPrice, decimal minimumCharge)
        {
            return new BillingRate
            {
                Name = BillingRate.HourlyType,
                UnitPrice = unitPrice,
                MinimumCharge = minimumCharge
            };
        }

        public static BillingRate CreateFlatRate(decimal unitPrice)
        {
            return new BillingRate
            {
                Name = BillingRate.FlatRateType,
                UnitPrice = unitPrice,
                MinimumCharge = unitPrice
            };
        }

        protected override bool EqualsCore(BillingRate other) =>
            Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase)
            && UnitPrice == other.UnitPrice;

        public override int GetHashCodeCore()
        {
            unchecked
            {
                int hash = 397;
                hash = hash * 397 + Name.GetHashCode();
                hash = hash * 397 + (int)UnitPrice;
                return hash;
            }
        }
    }
}