using System;

namespace ISTS.Application.Common
{
    public class BillingRateDto
    {
        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumCharge { get; set; }
    }
}