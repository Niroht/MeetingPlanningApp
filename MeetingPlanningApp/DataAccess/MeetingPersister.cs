using DataAccess.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Common.Model;

namespace DataAccess
{
    public class MeetingPersister : IMeetingPersister
    {
        private readonly IMeetingStore _meetingStore;

        public Task SaveMeeting(Meeting meeting)
        {
            var existingMeeting = _meetingStore.GetMeetings().FirstOrDefault(x => x.Id == meeting.Id);

            if (existingMeeting != null)
            {
                _meetingStore.UpdateMeeting(meeting);
            }
            else
            {
                _meetingStore.AddNewMeeting(meeting);
            }

            return Task.CompletedTask;
        }

        public MeetingPersister(IMeetingStore meetingStore)
        {
            _meetingStore = meetingStore;
        }
    }
}
