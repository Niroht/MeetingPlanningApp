using DataAccess.Common.Model;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Azure
{
    class MeetingEntity : TableEntity
    {
        public Guid Id { get; set; }

        public DateTime ScheduledTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Title { get; set; }

        public string Agenda { get; set; }

        public string Attendees { get; set; }

        public MeetingEntity(string  meetingId) : base("meeting", meetingId)
        {
        }

        public MeetingEntity()
        {
        }

        internal Meeting ToMeeting()
        {
            return new Meeting(ScheduledTime, Title, Agenda, JsonConvert.DeserializeObject<IEnumerable<Attendee>>(Attendees), EndTime - ScheduledTime, Id);
        }
    }
}
