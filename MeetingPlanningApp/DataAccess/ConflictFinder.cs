using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Common.Model;
using DataAccess.Common.Interfaces;

namespace DataAccess
{
    public class ConflictFinder : IConflictFinder
    {
        private readonly IMeetingProvider _meetingProvider;

        public ConflictFinder(IMeetingProvider meetingProvider)
        {
            _meetingProvider = meetingProvider;
        }

        public async Task<IEnumerable<AttendeeConflicts>> FindConflictsAsync(IEnumerable<Attendee> attendees, DateTime scheduledTime, DateTime endTime, Guid?meetingId = null)
        {
            var existingMeetings = await _meetingProvider.GetMeetings(scheduledTime.Date, endTime.Date);

            var conflicts = new List<AttendeeConflicts>();

            foreach(var meeting in existingMeetings.Where(x => x.Id != meetingId && MeetingConflicts(x, scheduledTime, endTime)))
            {
                AddAttendeeConflictsForMeeting(attendees, meeting, conflicts);
            }

            return conflicts;
        }

        private bool MeetingConflicts(Meeting existingMeeting, DateTime scheduledTime, DateTime endTime)
        {
            var isBeginningTimeInConflict = existingMeeting.ScheduledTime > scheduledTime && existingMeeting.ScheduledTime < endTime;
            var isEndTimeInConflict = existingMeeting.EndTime> scheduledTime && existingMeeting.EndTime < endTime;

            var isCompleteOverlap = existingMeeting.ScheduledTime < scheduledTime && existingMeeting.EndTime > endTime;

            return isBeginningTimeInConflict || isEndTimeInConflict || isCompleteOverlap;
        }

        private void AddAttendeeConflictsForMeeting(
            IEnumerable<Attendee> attendees, 
            Meeting existingMeeting,
            List<AttendeeConflicts> attendeeConflicts)
        {
            foreach (var attendee in attendees)
            {
                var isAttendeeInMeeting = existingMeeting.Attendees.Any(x => x.Email.Equals(attendee.Email, StringComparison.CurrentCultureIgnoreCase));

                if (!isAttendeeInMeeting)
                {
                    continue;
                }

                var existingConflict = attendeeConflicts.FirstOrDefault(x => x.Attendee.Email == attendee.Email);

                if (existingConflict != null)
                {
                    existingConflict.ConflictingMeetings.Add(existingMeeting);
                }
                else
                {
                    attendeeConflicts.Add(new AttendeeConflicts(attendee, existingMeeting));
                }
            }
        }
    }
}
