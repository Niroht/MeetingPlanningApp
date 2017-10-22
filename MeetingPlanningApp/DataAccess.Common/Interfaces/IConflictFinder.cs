using DataAccess.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common.Interfaces
{
    public interface IConflictFinder
    {
        Task<IEnumerable<AttendeeConflicts>> FindConflictsAsync(IEnumerable<Attendee> attendees, DateTime scheduledTime, TimeSpan duration, Guid? meetingId = null);
    }
}
