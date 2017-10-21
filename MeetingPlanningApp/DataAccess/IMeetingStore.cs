using DataAccess.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IMeetingStore
    {
        IEnumerable<Meeting> GetMeetings();

        void UpdateMeeting(Meeting meeting);

        void AddNewMeeting(Meeting meeting);

        void DeleteMeeting(Guid meetingId);
    }
}
