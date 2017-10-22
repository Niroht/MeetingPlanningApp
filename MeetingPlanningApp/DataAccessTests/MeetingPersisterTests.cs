using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using DataAccess;
using Moq;
using DataAccess.Common.Model;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using DataAccess.Messages;

namespace DataAccessTests
{
    [TestFixture]
    public class MeetingPersisterTests
    {
        [Test]
        public void UpdateExistingMeeting()
        {
            // arrange
            var tc = new TestContext();

            var meetings = new[] { new Meeting(DateTime.Today.AddDays(1), "a", "a", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1)) };
            tc.MeetingStoreMock.Setup(x => x.GetMeetings()).Returns(meetings);

            // act
            tc.Sut.SaveMeeting(new Meeting(DateTime.Today.AddDays(1), "b", "b", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1), meetings[0].Id));

            // assert
            tc.MeetingStoreMock.Verify(x => x.UpdateMeeting(It.Is<Meeting>(meeting => meeting.Id == meetings[0].Id)));
            tc.MeetingStoreMock.Verify(x => x.AddNewMeeting(It.IsAny<Meeting>()), Times.Never);
        }

        [Test]
        public void UpdateExistingMeeting_AlertsOfMeetingUpdated()
        {
            // arrange
            var tc = new TestContext();

            var meetings = new[] { new Meeting(DateTime.Today.AddDays(1), "a", "a", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1)) };
            tc.MeetingStoreMock.Setup(x => x.GetMeetings()).Returns(meetings);

            // act
            tc.Sut.SaveMeeting(new Meeting(DateTime.Today.AddDays(1), "b", "b", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1), meetings[0].Id));

            // assert
            tc.MessengerMock.Verify(x => x.Send(It.IsAny<MeetingUpdatedMessage>()));
        }

        [Test]
        public void DeleteMeeting()
        {
            // arrange
            var tc = new TestContext();

            var id = Guid.NewGuid();
            var meeting = new Meeting(DateTime.Now, "a", "a", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1), id);

            // act
            tc.Sut.DeleteMeeting(meeting);

            // assert
            tc.MeetingStoreMock.Verify(x => x.DeleteMeeting(id));
        }

        [Test]
        public void DeleteMeeting_AlertsOfDeletedMeeting()
        {
            // arrange
            var tc = new TestContext();

            var id = Guid.NewGuid();
            var meeting = new Meeting(DateTime.Now, "a", "a", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1), id);

            // act
            tc.Sut.DeleteMeeting(meeting);

            // assert
            tc.MessengerMock.Verify(x => x.Send(It.IsAny<MeetingDeletedMessage>()));
        }

        [Test]
        public void AddNewMeeting()
        {
            // arrange
            var tc = new TestContext();

            var meetings = new[] { new Meeting(DateTime.Today.AddDays(1), "a", "a", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1)) };
            tc.MeetingStoreMock.Setup(x => x.GetMeetings()).Returns(meetings);

            var newMeeting = new Meeting(DateTime.Today.AddDays(1), "b", "b", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1));

            // act
            tc.Sut.SaveMeeting(newMeeting);

            // assert
            tc.MeetingStoreMock.Verify(x => x.AddNewMeeting(It.Is<Meeting>(meeting => meeting.Id == newMeeting.Id)));
            tc.MeetingStoreMock.Verify(x => x.UpdateMeeting(It.IsAny<Meeting>()), Times.Never);
        }

        [Test]
        public void AddNewMeeting_AlertsOfNewMeetingCreated()
        {
            // arrange
            var tc = new TestContext();

            var meetings = new[] { new Meeting(DateTime.Today.AddDays(1), "a", "a", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1)) };
            tc.MeetingStoreMock.Setup(x => x.GetMeetings()).Returns(meetings);

            var newMeeting = new Meeting(DateTime.Today.AddDays(1), "b", "b", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1));

            // act
            tc.Sut.SaveMeeting(newMeeting);

            // assert
            tc.MessengerMock.Verify(x => x.Send(It.IsAny<MeetingCreatedMessage>()));
        }

        private class TestContext
        {
            public TestContext()
            {
                MeetingStoreMock = new Mock<IMeetingStore>();
                MessengerMock = new Mock<IMessenger>();

                Sut = new MeetingPersister(MeetingStoreMock.Object, MessengerMock.Object);
            }

            public MeetingPersister Sut { get; }

            public Mock<IMeetingStore> MeetingStoreMock { get; }

            public Mock<IMessenger> MessengerMock { get; }
        }
    }
}
