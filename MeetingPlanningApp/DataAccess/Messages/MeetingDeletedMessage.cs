using DataAccess.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Messages
{
    public class MeetingDeletedMessage
    {
        public Meeting DeletedMeeting { get; set; }

        public MeetingDeletedMessage(Meeting deletedMeeting)
        {
            DeletedMeeting = deletedMeeting;
        }
    }
}
