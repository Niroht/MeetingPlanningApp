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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetingPlanningApp.ViewModel
{
    public class ScheduleOverviewViewModel : ViewModelBase
    {
        private readonly IMonthViewModelFactory _monthViewModelFactory;
        private readonly IMeetingViewModelFactory _meetingViewModelFactory;
        private readonly IMeetingProvider _meetingProvider;
        private readonly IViewModelRenderer _viewModelRenderer;
        private readonly IModifyMeetingViewModelFactory _createMeetingViewModelFactory;

        private IEnumerable<MeetingViewModel> _upcomingMeetings;
        public IEnumerable<MeetingViewModel> UpcomingMeetings
        {
            get
            {
                return _upcomingMeetings;
            }
            set
            {
                Set(nameof(UpcomingMeetings), ref _upcomingMeetings, value);
            }
        }

        private MonthViewModel _monthlyMeetings;
        public MonthViewModel MonthlyMeetings
        {
            get
            {
                return _monthlyMeetings;
            }
            set
            {
                Set(nameof(MonthlyMeetings), ref _monthlyMeetings, value);
            }
        }

        public ICommand NewMeetingCommand => new RelayCommand(() => _viewModelRenderer.RenderViewModelInModal(_createMeetingViewModelFactory.Create()));

        public ICommand RefreshCommand => new RelayCommand(async () => await LoadMeetings());

        public ScheduleOverviewViewModel(
            IMeetingProvider meetingProvider, 
            IMessenger messenger,
            IViewModelRenderer viewModelRenderer,
            IModifyMeetingViewModelFactory createMeetingViewModelFactory,
            IMeetingViewModelFactory meetingViewModelFactory,
            IMonthViewModelFactory monthViewModelFactory)
        {
            _meetingProvider = meetingProvider;
            _viewModelRenderer = viewModelRenderer;
            _createMeetingViewModelFactory = createMeetingViewModelFactory;
            _meetingViewModelFactory = meetingViewModelFactory;
            _monthViewModelFactory = monthViewModelFactory;

            messenger.Register<MeetingCreatedMessage>(this, async x => await LoadMeetings());
            messenger.Register<MeetingUpdatedMessage>(this, async x => await LoadMeetings());
            messenger.Register<MeetingDeletedMessage>(this, async x => await LoadMeetings());

            LoadMeetings();
        }

        private async Task LoadMeetings()
        {
            UpcomingMeetings = (await _meetingProvider
                .GetMeetings(DateTime.Today, DateTime.Today.AddDays(3)))
                .Select(x => _meetingViewModelFactory.Create(x));

            MonthlyMeetings = await _monthViewModelFactory.Create();
        }
    }
}
