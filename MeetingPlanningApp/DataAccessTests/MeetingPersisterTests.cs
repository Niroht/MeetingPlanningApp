using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using DataAccess;
using Moq;
using DataAccess.Common.Model;

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

            var meetings = new[] { new Meeting(DateTime.Today.AddDays(1), "a", "a") };
            tc.MeetingStoreMock.SetupGet(x => x.MockMeetingData).Returns(meetings);

            // act
            tc.Sut.SaveMeeting(new Meeting(DateTime.Today.AddDays(1), "b", "b", meetings[0].Id));

            // assert
            tc.MeetingStoreMock.Verify(x => x.UpdateMeeting(It.Is<Meeting>(meeting => meeting.Id == meetings[0].Id)));
            tc.MeetingStoreMock.Verify(x => x.AddNewMeeting(It.IsAny<Meeting>()), Times.Never);
        }

        [Test]
        public void AddNewMeeting()
        {
            // arrange
            var tc = new TestContext();

            var meetings = new[] { new Meeting(DateTime.Today.AddDays(1), "a", "a") };
            tc.MeetingStoreMock.SetupGet(x => x.MockMeetingData).Returns(meetings);

            var newMeeting = new Meeting(DateTime.Today.AddDays(1), "b", "b");

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
