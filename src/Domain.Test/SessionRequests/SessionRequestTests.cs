using System;

using Xunit;

using ISTS.Domain.Common;
using ISTS.Domain.SessionRequests;

namespace ISTS.Domain.Test.SessionRequests
{
    public class SessionRequestTests
    {
        [Theory]
        [InlineData("Approved", "Reject")]
        [InlineData("Approved", "Approve")]
        [InlineData("Rejected", "Approve")]
        [InlineData("Rejected", "Reject")]
        public void ApproveReject_Throws_Exception_When_Request_Not_Pending(string currentState, string action)
        {
            var request = SessionRequest.Create(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now);
            switch (currentState)
            {
                case "Approved":
                    request.Approve();
                    break;
                case "Rejected":
                    request.Reject("SomeReason");
                    break;
            }

            Assert.Throws<DomainValidationException>(() =>
            {
                switch (action)
                {
                    case "Approve":
                        request.Approve();
                        break;
                    case "Reject":
                        request.Reject("Some reason");
                        break;
                }
            });
        }
    }
}