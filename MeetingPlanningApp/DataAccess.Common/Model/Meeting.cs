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

        public string Title { get; }

        public string Agenda { get; }

        public IEnumerable<Attendant> Attendants { get; }

        public Meeting(DateTime scheduledTime, string title, string agenda, IEnumerable<Attendant> attendants, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            ScheduledTime = scheduledTime;
            Title = title;
            Agenda = agenda;
            Attendants = attendants;
        }
    }
}
