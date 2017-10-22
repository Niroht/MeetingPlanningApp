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

        public async Task<IEnumerable<AttendeeConflicts>> FindConflictsAsync(IEnumerable<Attendee> attendees, DateTime scheduledTime, TimeSpan duration, Guid?meetingId = null)
        {
            var existingMeetings = await _meetingProvider.GetMeetings(scheduledTime.Date, scheduledTime.Add(duration).Date);

            var conflicts = new List<AttendeeConflicts>();

            foreach(var meeting in existingMeetings.Where(x => x.Id != meetingId && MeetingConflicts(x, scheduledTime, duration)))
            {
                AddAttendeeConflictsForMeeting(attendees, meeting, conflicts);
            }

            return conflicts;
        }

        private bool MeetingConflicts(Meeting existingMeeting, DateTime scheduledTime, TimeSpan duration)
        {
            var isBeginningTimeInConflict = existingMeeting.ScheduledTime > scheduledTime && existingMeeting.ScheduledTime < scheduledTime.Add(duration);
            var isEndTimeInConflict = existingMeeting.ScheduledTime.Add(existingMeeting.Duration) > scheduledTime && existingMeeting.ScheduledTime.Add(existingMeeting.Duration) < scheduledTime.Add(duration);

            var isCompleteOverlap = existingMeeting.ScheduledTime < scheduledTime && existingMeeting.ScheduledTime.Add(existingMeeting.Duration) > scheduledTime.Add(duration);

            return isBeginningTimeInConflict || isEndTimeInConflict || isCompleteOverlap;
        }

        private void AddAttendeeConflictsForMeeting(
            IEnumerable<Attendee> attendees, 
            Meeting existingMeeting,
            List<AttendeeConflicts> attendeeConflicts)
        {
            foreach (var attendee in attendees)
            {
                var isAttendeeInMeeting = existingMeeting.Attendees.Any(x => x.Email == attendee.Email);

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
