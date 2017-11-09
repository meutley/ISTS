using System;

using Xunit;

using ISTS.Domain.Common;
using ISTS.Domain.Rooms;

namespace ISTS.Domain.Test.Rooms
{
    public class RoomFunctionTests
    {
        [Theory]
        [InlineData("Some function", null)]
        [InlineData("Some function", "")]
        [InlineData("Some function", "        ")]
        public void Create_Returns_New_RoomFunction_And_BillingRate_Is_None(string name, string description)
        {
            var roomId = Guid.NewGuid();

            var model = RoomFunction.Create(name, description, roomId);
            Assert.Equal(name, model.Name);
            Assert.Equal(description, model.Description);
            Assert.Equal(roomId, model.RoomId);
            Assert.Equal(BillingRate.None, model.BaseBillingRate);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("        ")]
        public void Create_When_Name_IsNullOrEmpty_Throws_DomainValidationException(string name)
        {
            var ex = Assert.Throws<DomainValidationException>(() => RoomFunction.Create(name, string.Empty, Guid.NewGuid()));
            Assert.NotNull(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<ArgumentException>(ex.InnerException);
        }

        [Theory]
        [InlineData("", 0, 0)]
        [InlineData("", 25, 0)]
        [InlineData("", 25, 75)]
        [InlineData("Hourly", 25, 50)]
        [InlineData("FlatRate", 25, 0)]
        [InlineData("FlatRate", 25, 100)]
        public void SetBillingRate_Sets_BaseBillingRate_By_Type(string type, decimal unitPrice, decimal minimumCharge)
        {
            var roomFunction = RoomFunction.Create("Some function", string.Empty, Guid.NewGuid());
            roomFunction.SetBillingRate(type, unitPrice, minimumCharge);
            
            var billingRate = roomFunction.BaseBillingRate;
            switch (billingRate.Name)
            {
                case BillingRate.NoneType:
                    Assert.Equal(0, billingRate.UnitPrice);
                    Assert.Equal(0, billingRate.MinimumCharge);
                    break;
                case BillingRate.HourlyType:
                    Assert.Equal(unitPrice, billingRate.UnitPrice);
                    Assert.Equal(minimumCharge, billingRate.MinimumCharge);
                    break;
                case BillingRate.FlatRateType:
                    Assert.Equal(unitPrice, billingRate.UnitPrice);
                    Assert.Equal(unitPrice, billingRate.MinimumCharge);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected billing rate type: {type}");
            }
        }
    }
}