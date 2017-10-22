using DataAccess.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Common.Model;

namespace DataAccess
{
    public class MeetingProvider : IMeetingProvider
    {
        private readonly IMeetingStore _meetingStore;

        public Task<IEnumerable<Meeting>> GetMeetings(DateTime startDate, DateTime endDate)
        {
            var meetings = _meetingStore.GetMeetings();
            var filteredMeetings = meetings.Where(x => x.ScheduledTime.Date >= startDate.Date && x.EndTime.Date <= endDate.Date);

            return Task.FromResult(filteredMeetings);
        }

        public MeetingProvider(IMeetingStore meetingStore)
        {
            _meetingStore = meetingStore;
        }
    }
}
