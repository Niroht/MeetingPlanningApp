using DataAccess.Common.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MeetingPlanningApp.Factory;
using MeetingPlanningApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetingPlanningApp.ViewModel
{
    public class MeetingViewModel : ViewModelBase
    {
        private readonly IModifyMeetingViewModelFactory _modifyMeetingViewModelFactory;
        private readonly IViewModelRenderer _viewModelRenderer;

        public Guid Id { get; }

        public DateTime ScheduledTime { get; set; }

        public string Title { get; set; }

        public string Agenda { get; set; }

        public IEnumerable<Attendant> Attendants { get; set; }

        public ICommand ModifyMeetingCommand => new RelayCommand(OnModifyMeeting);

        public MeetingViewModel(Meeting meeting,
            IViewModelRenderer viewModelRenderer,
            IModifyMeetingViewModelFactory modifyMeetingViewModelFactory)
        {
            _viewModelRenderer = viewModelRenderer;
            _modifyMeetingViewModelFactory = modifyMeetingViewModelFactory;

            Id = meeting.Id;
            ScheduledTime = meeting.ScheduledTime;
            Title = meeting.Title;
            Agenda = meeting.Agenda;
            Attendants = meeting.Attendees;
        }

        private void OnModifyMeeting()
        {
            var meeting = new Meeting(ScheduledTime, Title, Agenda, Attendants, Id);
            var modifyViewModel = _modifyMeetingViewModelFactory.Create(meeting);

            _viewModelRenderer.RenderViewModelInModal(modifyViewModel);
        }
    }
}
