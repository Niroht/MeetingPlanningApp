using DataAccess.Common.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingPlanningApp.ViewModel
{
    public class MeetingViewModel : ViewModelBase
    {
        public Guid Id { get; }

        public DateTime ScheduledTime { get; set; }

        public string Title { get; set; }

        public string Agenda { get; set; }

        public MeetingViewModel(Meeting meeting)
        {
            Id = meeting.Id;
            ScheduledTime = meeting.ScheduledTime;
            Title = meeting.Title;
            Agenda = meeting.Agenda;
        }
    }
}
