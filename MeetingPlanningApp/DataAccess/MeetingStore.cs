using DataAccess.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MeetingStore : IMeetingStore
    {
        private IList<Meeting> _mockMeetingData = new List<Meeting>
        {
            new Meeting(DateTime.Now.AddDays(3), "Test Meeting 1", "Prepare for meeting 2"),
            new Meeting(DateTime.Now.AddDays(5), "Test Meeting 2", "Discuss meeting 1; prepare for meeting 3"),
            new Meeting(DateTime.Now.AddDays(15), "Test Meeting 3", "Finish Having Meetings")
        };

        public IEnumerable<Meeting> GetMeetings()
        {
            return _mockMeetingData;
        }

        public void UpdateMeeting(Meeting meeting)
        {
            var existingMeeting = _mockMeetingData.First(x => x.Id == meeting.Id);

            existingMeeting = meeting;
        }

        public void AddNewMeeting(Meeting meeting)
        {
            _mockMeetingData.Add(meeting);
        }
    }
}
