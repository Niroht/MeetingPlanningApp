using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common.Model
{
    public class Meeting
    {
        public Guid Id { get; }

        public DateTime ScheduledTime { get; }

        public TimeSpan Duration { get; }

        public DateTime EndTime => ScheduledTime.Add(Duration);

        public string Title { get; }

        public string Agenda { get; }

        public IEnumerable<Attendee> Attendees { get; }

        public Meeting(DateTime scheduledTime, string title, string agenda, IEnumerable<Attendee> attendees, TimeSpan duration, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            ScheduledTime = scheduledTime;
            Title = title;
            Agenda = agenda;
            Attendees = attendees;
            Duration = duration;
        }
    }
}
