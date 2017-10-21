using DataAccess.Common.Interfaces;
using DataAccess.Common.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MeetingPlanningApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetingPlanningApp.ViewModel
{
    public class ModifyMeetingViewModel : ViewModelBase
    {
        private readonly IMeetingPersister _meetingPersister;
        private readonly IViewModelRenderer _viewModelRenderer;

        private Meeting _existingMeeting;
        public Meeting ExistingMeeting
        {
            get
            {
                return _existingMeeting;
            }
        }

        private DateTime _scheduledTime = DateTime.Today;
        public DateTime ScheduledTime
        {
            get
            {
                return _scheduledTime;
            }
            set
            {
                Set(nameof(ScheduledTime), ref _scheduledTime, value);
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(nameof(Title), ref _title, value);
            }
        }

        private string _agenda;
        public string Agenda
        {
            get
            {
                return _agenda;
            }
            set
            {
                Set(nameof(Agenda), ref _agenda, value);
            }
        }

        private IEnumerable<Attendant> _attendees;
        public IEnumerable<Attendant> Attendees
        {
            get
            {
                return _attendees;
            }
            set
            {
                Set(nameof(Attendees), ref _attendees, value);
            }
        }

        public ICommand DeleteMeetingCommand => new RelayCommand(async () => await DeleteMeeting());

        public ICommand SaveNewMeetingCommand => new RelayCommand(async () => await SaveMeetingAsync());

        public ModifyMeetingViewModel(
            IMeetingPersister meetingPersister,
            IViewModelRenderer viewModelRenderer)
        {
            _meetingPersister = meetingPersister;
            _viewModelRenderer = viewModelRenderer;
        }

        public ModifyMeetingViewModel(
            Meeting meeting,
            IMeetingPersister meetingPersister,
            IViewModelRenderer viewModelRenderer)
        {
            _meetingPersister = meetingPersister;
            _viewModelRenderer = viewModelRenderer;

            if (meeting != null)
            {
                _existingMeeting = meeting;
                ScheduledTime = meeting.ScheduledTime;
                Title = meeting.Title;
                Agenda = meeting.Agenda;
                Attendees = meeting.Attendees;
            }
        }

        private async Task SaveMeetingAsync()
        {
            var meeting = _existingMeeting == null ? new Meeting(ScheduledTime, Title, Agenda, Attendees) : new Meeting(ScheduledTime, Title, Agenda, Attendees, _existingMeeting.Id);

            await _meetingPersister.SaveMeeting(meeting);
            _viewModelRenderer.HideModal();
        }

        private async Task DeleteMeeting()
        {
            await _meetingPersister.DeleteMeeting(_existingMeeting);
            _viewModelRenderer.HideModal();
        }
    }
}
