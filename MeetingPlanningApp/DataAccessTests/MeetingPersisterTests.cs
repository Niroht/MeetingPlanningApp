using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using DataAccess;
using Moq;
using DataAccess.Common.Model;
using System.Linq;

namespace DataAccessTests
{
    [TestFixture]
    public class MeetingPersisterTests
    {
        [Test]
        public void SaveExistingMeeting()
        {
            // arrange
            var tc = new TestContext();

            var meetings = new[] { new Meeting(DateTime.Today.AddDays(1), "a", "a", Enumerable.Empty<Attendant>()) };
            tc.MeetingStoreMock.Setup(x => x.GetMeetings()).Returns(meetings);

            // act
            tc.Sut.SaveMeeting(new Meeting(DateTime.Today.AddDays(1), "b", "b", Enumerable.Empty<Attendant>(), meetings[0].Id));

            // assert
            tc.MeetingStoreMock.Verify(x => x.UpdateMeeting(It.Is<Meeting>(meeting => meeting.Id == meetings[0].Id)));
            tc.MeetingStoreMock.Verify(x => x.AddNewMeeting(It.IsAny<Meeting>()), Times.Never);
        }

        [Test]
        public void AddNewMeeting()
        {
            // arrange
            var tc = new TestContext();

            var meetings = new[] { new Meeting(DateTime.Today.AddDays(1), "a", "a", Enumerable.Empty<Attendant>()) };
            tc.MeetingStoreMock.Setup(x => x.GetMeetings()).Returns(meetings);

            var newMeeting = new Meeting(DateTime.Today.AddDays(1), "b", "b", Enumerable.Empty<Attendant>());

            // act
            tc.Sut.SaveMeeting(newMeeting);

            // assert
            tc.MeetingStoreMock.Verify(x => x.AddNewMeeting(It.Is<Meeting>(meeting => meeting.Id == newMeeting.Id)));
            tc.MeetingStoreMock.Verify(x => x.UpdateMeeting(It.IsAny<Meeting>()), Times.Never);
        }

        private class TestContext
        {
            public TestContext()
            {
                MeetingStoreMock = new Mock<IMeetingStore>();

                Sut = new MeetingPersister(MeetingStoreMock.Object);
            }

            public MeetingPersister Sut { get; }

            public Mock<IMeetingStore> MeetingStoreMock { get; }
        }
    }
}
