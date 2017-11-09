using System;

using Xunit;

using ISTS.Domain.Common;

namespace ISTS.Domain.Test.Common
{
    public class BillingRateTests
    {
        [Fact]
        public void BillingRate_None_Returns_Default_None()
        {
            var none = BillingRate.None;
            Assert.Equal(BillingRate.NoneType, none.Name);
            Assert.Equal(0, none.UnitPrice);
            Assert.Equal(0, none.MinimumCharge);
        }

        [Fact]
        public void CreateHourly_Returns_Hourly_BillingRate()
        {
            var hourly = BillingRate.CreateHourly(25, 75);
            Assert.Equal(BillingRate.HourlyType, hourly.Name);
            Assert.Equal(25, hourly.UnitPrice);
            Assert.Equal(75, hourly.MinimumCharge);
        }

        [Fact]
        public void CreateFlatRate_Returns_FlatRate_BillingRate()
        {
            var flatRate = BillingRate.CreateFlatRate(100);
            Assert.Equal(BillingRate.FlatRateType, flatRate.Name);
            Assert.Equal(100, flatRate.UnitPrice);
            Assert.Equal(flatRate.UnitPrice, flatRate.MinimumCharge);
        }
    }
}