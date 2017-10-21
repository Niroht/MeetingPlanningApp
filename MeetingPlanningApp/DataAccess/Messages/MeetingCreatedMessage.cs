using DataAccess.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Messages
{
    public class MeetingCreatedMessage
    {
        public Meeting AddedMeeting { get; }

        public MeetingCreatedMessage(Meeting addedMeeting)
        {
            AddedMeeting = addedMeeting;
        }
    }
}
