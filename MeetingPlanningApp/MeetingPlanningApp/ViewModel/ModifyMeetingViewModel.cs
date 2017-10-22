using DataAccess.Common.Interfaces;
using DataAccess.Common.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MeetingPlanningApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MeetingPlanningApp.ViewModel
{
    public class ModifyMeetingViewModel : ViewModelBase
    {
        private readonly IConflictFinder _conflictFinder;
        private readonly IMeetingProvider _meetingProvider;
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

        public IEnumerable<int> HourValues => Enumerable.Range(1, 24);

        public IEnumerable<int> MinuteValues => Enumerable.Range(1, 60);

        private DateTime _scheduledDateTime = DateTime.Now.AddHours(1);
        public DateTime ScheduledDay
        {
            get
            {
                return _scheduledDateTime.Date;
            }
            set
            {
                _scheduledDateTime = new DateTime(value.Year, value.Month, value.Day, ScheduledHour, ScheduledMinute, 0);
                MeetingTimeChanged();
            }
        }

        public int ScheduledHour
        {
            get
            {
                return _scheduledDateTime.Hour;
            }
            set
            {
                _scheduledDateTime = new DateTime(_scheduledDateTime.Year, _scheduledDateTime.Month, _scheduledDateTime.Day, value, ScheduledMinute, 0);
                MeetingTimeChanged();
            }
        }

        public int ScheduledMinute
        {
            get
            {
                return _scheduledDateTime.Minute;
            }
            set
            {
                _scheduledDateTime = new DateTime(_scheduledDateTime.Year, _scheduledDateTime.Month, _scheduledDateTime.Day, ScheduledHour, value, 0);
                MeetingTimeChanged();
            }
        }

        private TimeSpan _duration = TimeSpan.FromHours(1);
        public int DurationHours
        {
            get
            {
                return _duration.Hours;
            }
            set
            {
                _duration = new TimeSpan(value, _duration.Minutes, 0);
                MeetingTimeChanged();
            }
        }

        public int DurationMinutes
        {
            get
            {
                return _duration.Minutes;
            }
            set
            {
                _duration = new TimeSpan(_duration.Hours, value, 0);
                MeetingTimeChanged();
            }
        }

        public DateTime EndTime
        {
            get
            {
                return _scheduledDateTime.Add(_duration);
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

        private ObservableCollection<Attendee> _attendees = new ObservableCollection<Attendee>();
        public ObservableCollection<Attendee> Attendees
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

        private string _newAttendeeEmail;
        public string NewAttendeeEmail
        {
            get
            {
                return _newAttendeeEmail;
            }
            set
            {
                Set(nameof(NewAttendeeEmail), ref _newAttendeeEmail, value);
            }
        }

        private string _newAttendeeName;
        public string NewAttendeeName
        {
            get
            {
                return _newAttendeeName;
            }
            set
            {
                Set(nameof(NewAttendeeName), ref _newAttendeeName, value);
            }
        }

        private string _conflictMessage;
        public string ConflictMessage
        {
            get
            {
                return _conflictMessage;
            }
            set
            {
                Set(nameof(ConflictMessage), ref _conflictMessage, value);
            }
        }

        public Visibility ConflictMessageVisibility
        {
            get
            {
                return string.IsNullOrWhiteSpace(ConflictMessage) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private bool _canProceed;
        public bool CanProceed
        {
            get
            {
                return _canProceed;
            }
            set
            {
                Set(nameof(CanProceed), ref _canProceed, value);
            }
        }

        public ICommand RemoveAttendeeCommand => new RelayCommand<Attendee>(async x => await RemoveAttendee(x));

        public ICommand DeleteMeetingCommand => new RelayCommand(async () => await DeleteMeeting());

        public ICommand AddAttendeeCommand => new RelayCommand(async () => await AddAttendee(), CanAddAttendee);

        public ICommand SaveNewMeetingCommand => new RelayCommand(async () => await SaveMeetingAsync(), () => CanProceed);

        public ModifyMeetingViewModel(
            IMeetingPersister meetingPersister,
            IConflictFinder conflictFinder,
            IViewModelRenderer viewModelRenderer)
        {
            _meetingPersister = meetingPersister;
            _viewModelRenderer = viewModelRenderer;
            _conflictFinder = conflictFinder;

            IdentifyConflicts();
        }

        public ModifyMeetingViewModel(
            Meeting meeting,
            IMeetingPersister meetingPersister,
            IConflictFinder conflictFinder,
            IViewModelRenderer viewModelRenderer)
        {
            _meetingPersister = meetingPersister;
            _viewModelRenderer = viewModelRenderer;
            _conflictFinder = conflictFinder;

            if (meeting != null)
            {
                _existingMeeting = meeting;
                _scheduledDateTime = meeting.ScheduledTime;
                _duration = meeting.Duration;
                ScheduledDay = meeting.ScheduledTime;
                Title = meeting.Title;
                Agenda = meeting.Agenda;
                Attendees = new ObservableCollection<Attendee>(meeting.Attendees);
            }

            IdentifyConflicts();
        }

        private async Task RemoveAttendee(Attendee attendee)
        {
            Attendees.Remove(attendee);

            await IdentifyConflicts();
        }

        private async Task AddAttendee()
        {
            Attendees.Add(new Attendee(NewAttendeeName, NewAttendeeEmail));

            await IdentifyConflicts();
        }

        private async Task MeetingTimeChanged()
        {
            RaisePropertyChanged(nameof(EndTime));

            await IdentifyConflicts();
        }

        private async Task IdentifyConflicts()
        {
            var conflicts = await _conflictFinder.FindConflictsAsync(Attendees, _scheduledDateTime, _scheduledDateTime.Add(_duration), _existingMeeting?.Id);

            if (conflicts.Any())
            {
                CanProceed = false;
                ConflictMessage = $"The following attendees have conflicts: {string.Join(",", conflicts.Select(x => x.Attendee.Name))}";
            }
            else
            {
                CanProceed = true;
                ConflictMessage = string.Empty;
                RaisePropertyChanged(nameof(ConflictMessage));
            }
        }

        private async Task SaveMeetingAsync()
        {
            var meeting = _existingMeeting == null ? new Meeting(_scheduledDateTime, Title, Agenda, Attendees, _duration) : new Meeting(_scheduledDateTime, Title, Agenda, Attendees, _duration, _existingMeeting.Id);

            await _meetingPersister.SaveMeeting(meeting);
            _viewModelRenderer.HideModal();
        }

        private async Task DeleteMeeting()
        {
            await _meetingPersister.DeleteMeeting(_existingMeeting);
            _viewModelRenderer.HideModal();
        }

        private bool CanAddAttendee()
        {
            return !string.IsNullOrWhiteSpace(NewAttendeeName)
                && !string.IsNullOrWhiteSpace(NewAttendeeEmail)
                && !Attendees.Select(x => x.Email).Contains(NewAttendeeEmail);
        }
    }
}
