using DataAccess.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Messages
{
    public class MeetingUpdatedMessage
    {
        public Meeting UpdatedMeeting { get; }

        public MeetingUpdatedMessage(Meeting updatedMeeting)
        {
            UpdatedMeeting = updatedMeeting;
        }
    }
}
