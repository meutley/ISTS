using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using Moq;

using ISTS.Application.Sessions;
using ISTS.Domain.Sessions;
using ISTS.Domain.Studios;

namespace ISTS.Application.Tests.Sessions
{
    public class SessionScheduleEntryRepositoryTests
    {
        private readonly Studio _studio;
        private readonly Session _session;

        private readonly Mock<ISessionScheduleEntryRepository> _repository;

        public SessionScheduleEntryRepositoryTests()
        {
            _studio = Studio.Create();
            _session = Session.Create(_studio);
            
            _repository = new Mock<ISessionScheduleEntryRepository>();
        }

        [Fact]
        public void GetCurrentSchedule_Returns_A_Single_Entity()
        {
            var startDate = DateTime.Now;
            var endDate = startDate.AddHours(1.5);

            var sessionScheduleEntry = SessionScheduleEntry.Create(_session, startDate, endDate);

            _repository
                .Setup(x => x.GetCurrentSchedule(It.IsAny<Guid>()))
                .Returns(sessionScheduleEntry);

            var entry = _repository.Object.GetCurrentSchedule(Guid.NewGuid());

            Assert.NotNull(entry);
            Assert.Equal(sessionScheduleEntry.Id, entry.Id);
        }

        [Fact]
        public void GetAllSchedules_Returns_Multiple_Entities()
        {
            var startDate = DateTime.Now;
            var endDate = startDate.AddHours(1.5);

            var sessionScheduleEntry1 = SessionScheduleEntry.Create(_session, startDate, endDate);
            var sessionScheduleEntry2 = SessionScheduleEntry.Create(_session, startDate.AddDays(1), endDate.AddDays(1));

            _repository
                .Setup(x => x.GetAllSchedules(It.IsAny<Guid>()))
                .Returns(new List<SessionScheduleEntry>
                {
                    sessionScheduleEntry1,
                    sessionScheduleEntry2
                });

            var entries = _repository.Object.GetAllSchedules(Guid.NewGuid());

            Assert.True(entries.Any());
            Assert.Equal(2, entries.Count());
            Assert.Equal(sessionScheduleEntry1.Id, entries.ElementAt(0).Id);
            Assert.Equal(sessionScheduleEntry2.Id, entries.ElementAt(1).Id);
        }
    }
}