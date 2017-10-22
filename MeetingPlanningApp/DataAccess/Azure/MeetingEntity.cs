using DataAccess.Common.Model;
using Microsoft.WindowsAzure.Storage.Table;
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
            var attendeeStrings = Attendees.Split('|');
            var attendees = new List<Attendee>();

            foreach(var attendeeString in attendeeStrings)
            {
                var parameters = attendeeString.Split(';');
                attendees.Add(new Attendee(parameters[0], parameters[1]));
            }

            return new Meeting(ScheduledTime, Title, Agenda, attendees, EndTime - ScheduledTime, Id);
        }
    }
}
