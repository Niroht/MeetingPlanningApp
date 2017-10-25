using DataAccess.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Azure
{
    internal static class AzureModelExtensions
    {
        public static MeetingEntity ToMeetingEntity(this Meeting meeting)
        {
            return new MeetingEntity(meeting.Id.ToString())
            {
                Id = meeting.Id,
                Title = meeting.Title,
                ScheduledTime = meeting.ScheduledTime,
                EndTime = meeting.EndTime,
                Attendees = JsonConvert.SerializeObject(meeting.Attendees),
                Agenda = meeting.Agenda,
            };
        }
    }
}
