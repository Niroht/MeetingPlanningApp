using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common.Model
{
    public class AttendeeConflicts
    {
        public Attendee Attendee { get; }

        public List<Meeting> ConflictingMeetings { get; } = new List<Meeting>();

        public AttendeeConflicts(Attendee attendee, Meeting conflictingMeeting)
        {
            Attendee = attendee;
            ConflictingMeetings.Add(conflictingMeeting);
        }
    }
}
