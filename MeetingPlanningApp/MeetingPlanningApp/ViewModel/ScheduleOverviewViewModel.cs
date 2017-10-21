using DataAccess.Common.Interfaces;
using DataAccess.Common.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetingPlanningApp.ViewModel
{
    public class ScheduleOverviewViewModel : ViewModelBase
    {
        private readonly IMeetingProvider _meetingProvider;
        private readonly IMeetingPersister _meetingPersister;

        private IEnumerable<MeetingViewModel> _allMeetingsInView;

        public IEnumerable<MeetingViewModel> UpcomingMeetings
        {
            get
            {
                return _allMeetingsInView.Where(x => (x.ScheduledTime - DateTime.Now).Days < 3);
            }
        }

        public IEnumerable<MeetingViewModel> CalendarMeetings
        {
            get
            {
                return _allMeetingsInView.Where(x => x.ScheduledTime.Month == DateTime.Now.Month);
            }
        }

        private DateTime _newMeetingDate = DateTime.Today;
        public DateTime NewMeetingDate
        {
            get
            {
                return _newMeetingDate;
            }
            set
            {
                Set(nameof(NewMeetingDate), ref _newMeetingDate, value);
            }
        }

        private string _newMeetingTitle;
        public string NewMeetingTitle
        {
            get
            {
                return _newMeetingTitle;
            }
            set
            {
                Set(nameof(NewMeetingTitle), ref _newMeetingTitle, value);
            }
        }

        private string _newMeetingAgenda;
        public string NewMeetingAgenda
        {
            get
            {
                return _newMeetingAgenda;
            }
            set
            {
                Set(nameof(NewMeetingAgenda), ref _newMeetingAgenda, value);
            }
        }

        private IEnumerable<Attendant> _newMeetingAttendants;
        public IEnumerable<Attendant> NewMeetingAttendants
        {
            get
            {
                return _newMeetingAttendants;
            }
            set
            {
                Set(nameof(NewMeetingAttendants), ref _newMeetingAttendants, value);
            }
        }

        public ICommand SaveNewMeetingCommand => new RelayCommand(async () => await SaveNewMeeting());

        public ICommand RefreshCommand => new RelayCommand(async () => await LoadMeetings());

        public ScheduleOverviewViewModel(IMeetingProvider meetingProvider, IMeetingPersister meetingPersister)
        {
            _meetingProvider = meetingProvider;
            _meetingPersister = meetingPersister;

            LoadMeetings();
        }

        private async Task LoadMeetings()
        {
            _allMeetingsInView = (await _meetingProvider
                .GetMeetings(DateTime.Today, DateTime.Today.AddDays(30)))
                .Select(x => new MeetingViewModel(x));

            RaisePropertyChanged(nameof(UpcomingMeetings));
            RaisePropertyChanged(nameof(CalendarMeetings));
        }

        private async Task SaveNewMeeting()
        {
            await _meetingPersister.SaveMeeting(new Meeting(NewMeetingDate, NewMeetingTitle, NewMeetingAgenda, NewMeetingAttendants));

            await LoadMeetings();
        }
    }
}
