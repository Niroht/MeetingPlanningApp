using DataAccess;
using DataAccess.Common.Interfaces;
using DataAccess.Common.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessTests
{
    [TestFixture]
    public class ConflictFinderTests
    {
        [Test]
        public async Task ConflictFinder_DoesNotFindConflictForCurrentMeeting()
        {
            // arrange
            var tc = new TestContext();

            var attendee = new Attendee() { Name = "a", Email = "x@y.z" };
            var existingMeeting = new Meeting(DateTime.Now.AddMinutes(30), "test", "", new[] { attendee }, TimeSpan.FromHours(1));

            tc.MeetingProviderMock.Setup(x => x.GetMeetings(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult<IEnumerable<Meeting>>(new[] { existingMeeting }));

            // act
            var result = await tc.Sut.FindConflictsAsync(new[] { attendee }, DateTime.Now, DateTime.Now.AddHours(1), existingMeeting.Id);

            // assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task ConflictFinder_FindsConflictingStartDate()
        {
            // arrange
            var tc = new TestContext();

            var attendee = new Attendee() { Name = "a", Email = "x@y.z" };
            var existingMeeting = new Meeting(DateTime.Now.AddMinutes(30), "test", "", new[] { attendee }, TimeSpan.FromHours(1));

            tc.MeetingProviderMock.Setup(x => x.GetMeetings(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult<IEnumerable<Meeting>>(new[] { existingMeeting }));

            // act
            var result = await tc.Sut.FindConflictsAsync(new[] { attendee }, DateTime.Now, DateTime.Now.AddHours(1), Guid.NewGuid());

            // assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(attendee.Email, result.First().Attendee.Email);
            Assert.AreEqual(existingMeeting.Title, result.First().ConflictingMeetings.First().Title);
        }

        [Test]
        public async Task ConflictFinder_FindsConflictingEndDate()
        {
            // arrange
            var tc = new TestContext();

            var attendee = new Attendee() { Name = "a", Email = "x@y.z" };
            var existingMeeting = new Meeting(DateTime.Now.Subtract(TimeSpan.FromMinutes(30)), "test", "", new[] { attendee }, TimeSpan.FromHours(1));

            tc.MeetingProviderMock.Setup(x => x.GetMeetings(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult<IEnumerable<Meeting>>(new[] { existingMeeting }));

            // act
            var result = await tc.Sut.FindConflictsAsync(new[] { attendee }, DateTime.Now, DateTime.Now.AddHours(1));

            // assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(attendee.Email, result.First().Attendee.Email);
            Assert.AreEqual(existingMeeting.Title, result.First().ConflictingMeetings.First().Title);
        }

        [Test]
        public async Task ConflictFinder_FindsConflictingWithinProposed()
        {
            // arrange
            var tc = new TestContext();

            var attendee = new Attendee() { Name = "a", Email = "x@y.z" };
            var existingMeeting = new Meeting(DateTime.Now.Add(TimeSpan.FromMinutes(15)), "test", "", new[] { attendee }, TimeSpan.FromMinutes(30));

            tc.MeetingProviderMock.Setup(x => x.GetMeetings(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult<IEnumerable<Meeting>>(new[] { existingMeeting }));

            // act
            var result = await tc.Sut.FindConflictsAsync(new[] { attendee }, DateTime.Now, DateTime.Now.AddHours(1));

            // assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(attendee.Email, result.First().Attendee.Email);
            Assert.AreEqual(existingMeeting.Title, result.First().ConflictingMeetings.First().Title);
        }

        [Test]
        public async Task ConflictFinder_FindsConflictingCompleteOverlap()
        {
            // arrange
            var tc = new TestContext();

            var attendee = new Attendee() { Name = "a", Email = "x@y.z" };
            var existingMeeting = new Meeting(DateTime.Now.Subtract(TimeSpan.FromMinutes(15)), "test", "", new[] { attendee }, TimeSpan.FromHours(2));

            tc.MeetingProviderMock.Setup(x => x.GetMeetings(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult<IEnumerable<Meeting>>(new[] { existingMeeting }));

            // act
            var result = await tc.Sut.FindConflictsAsync(new[] { attendee }, DateTime.Now, DateTime.Now.AddHours(1));

            // assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(attendee.Email, result.First().Attendee.Email);
            Assert.AreEqual(existingMeeting.Title, result.First().ConflictingMeetings.First().Title);
        }

        [Test]
        public async Task ConflictFinder_OneAttendeeConflictPerUser()
        {
            // arrange
            var tc = new TestContext();

            var attendee = new Attendee() { Name = "a", Email = "x@y.z" };
            var existingMeetings = new[] 
            {
                new Meeting(DateTime.Now.AddMinutes(30), "test", "", new[] { attendee }, TimeSpan.FromHours(1)),
                new Meeting(DateTime.Now.AddMinutes(30), "test", "", new[] { attendee }, TimeSpan.FromHours(1))
            };

            tc.MeetingProviderMock.Setup(x => x.GetMeetings(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult<IEnumerable<Meeting>>(existingMeetings));

            // act
            var result = await tc.Sut.FindConflictsAsync(new[] { attendee }, DateTime.Now, DateTime.Now.AddHours(1));

            // assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(2, result.First().ConflictingMeetings.Count());
        }

        [Test]
        public async Task ConflictFinder_FindsAllConflictingAttendees()
        {
            // arrange
            var tc = new TestContext();

            var attendeeOne = new Attendee() { Name = "a", Email = "x@y.z" };
            var attendeeTwo = new Attendee() { Name = "a", Email = "a@b.c" };
            var attendeeThree = new Attendee() { Name = "a", Email = "f@g.h" };
            var existingMeetings = new[]
            {
                new Meeting(DateTime.Now.AddMinutes(30), "test", "", new[] { attendeeOne, attendeeTwo }, TimeSpan.FromHours(1)),
            };

            tc.MeetingProviderMock.Setup(x => x.GetMeetings(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult<IEnumerable<Meeting>>(existingMeetings));

            // act
            var result = await tc.Sut.FindConflictsAsync(new[] { attendeeOne, attendeeTwo, attendeeThree }, DateTime.Now, DateTime.Now.AddHours(1));

            // assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, result.First().ConflictingMeetings.Count());
        }

        private class TestContext
        {
            public TestContext()
            {
                MeetingProviderMock = new Mock<IMeetingProvider>();

                Sut = new ConflictFinder(MeetingProviderMock.Object);
            }

            public ConflictFinder Sut { get; set; }

            public Mock<IMeetingProvider> MeetingProviderMock { get; }
        }
    }
}
