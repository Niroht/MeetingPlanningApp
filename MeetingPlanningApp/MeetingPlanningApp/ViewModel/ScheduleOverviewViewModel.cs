using DataAccess.Common.Interfaces;
using DataAccess.Common.Model;
using DataAccess.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class ScheduleOverviewViewModel : ViewModelBase
    {
        private readonly IMeetingViewModelFactory _meetingViewModelFactory;
        private readonly IMeetingProvider _meetingProvider;
        private readonly IViewModelRenderer _viewModelRenderer;
        private readonly IModifyMeetingViewModelFactory _createMeetingViewModelFactory;

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

        public ICommand NewMeetingCommand => new RelayCommand(() => _viewModelRenderer.RenderViewModelInModal(_createMeetingViewModelFactory.Create()));

        public ICommand RefreshCommand => new RelayCommand(async () => await LoadMeetings());

        public ScheduleOverviewViewModel(
            IMeetingProvider meetingProvider, 
            IMessenger messenger,
            IViewModelRenderer viewModelRenderer,
            IModifyMeetingViewModelFactory createMeetingViewModelFactory,
            IMeetingViewModelFactory meetingViewModelFactory)
        {
            _meetingProvider = meetingProvider;
            _viewModelRenderer = viewModelRenderer;
            _createMeetingViewModelFactory = createMeetingViewModelFactory;
            _meetingViewModelFactory = meetingViewModelFactory;

            messenger.Register<MeetingCreatedMessage>(this, async x => await LoadMeetings());
            messenger.Register<MeetingUpdatedMessage>(this, async x => await LoadMeetings());
            messenger.Register<MeetingDeletedMessage>(this, async x => await LoadMeetings());

            LoadMeetings();
        }

        private async Task LoadMeetings()
        {
            _allMeetingsInView = (await _meetingProvider
                .GetMeetings(DateTime.Today, DateTime.Today.AddDays(30)))
                .Select(x => _meetingViewModelFactory.Create(x));

            RaisePropertyChanged(nameof(UpcomingMeetings));
            RaisePropertyChanged(nameof(CalendarMeetings));
        }
    }
}
