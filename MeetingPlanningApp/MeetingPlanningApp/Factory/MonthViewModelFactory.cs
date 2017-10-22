using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingPlanningApp.ViewModel;
using DataAccess.Common.Interfaces;
using MeetingPlanningApp.Services;

namespace MeetingPlanningApp.Factory
{
    class MonthViewModelFactory : IMonthViewModelFactory
    {
        private readonly IModifyMeetingViewModelFactory _modifyMeetingViewModelFactory;
        private readonly IViewModelRenderer _viewModelRenderer;
        private readonly IMeetingViewModelFactory _meetingViewModelFactory;
        private readonly IMeetingProvider _meetingProvider;

        private const int NumberOfWeeksToRender = 5;
        public const int DaysPerWeek = 7;

        private readonly int NumberOfDaysToRender = NumberOfWeeksToRender * DaysPerWeek;

        public MonthViewModelFactory(
            IMeetingProvider meetingProvider,
            IMeetingViewModelFactory meetingViewModelFactory,
            IViewModelRenderer viewModelRenderer,
            IModifyMeetingViewModelFactory modifyMeetingViewModelFactory)
        {
            _meetingProvider = meetingProvider;
            _meetingViewModelFactory = meetingViewModelFactory;
            _viewModelRenderer = viewModelRenderer;
            _modifyMeetingViewModelFactory = modifyMeetingViewModelFactory;
        }

        public async Task<MonthViewModel> Create()
        {
            var days = await CreateDayViewModels();

            return new MonthViewModel(days, DateTime.Today.ToString("MMMM"));
        }

        private async Task<IEnumerable<DayViewModel>> CreateDayViewModels()
        {
            var todaysPosition = DateTime.Today.Day + (int)DateTime.Today.DayOfWeek;
            // Lower date range needs to take into account the current day, so add one day
            var lowerDateRange = DateTime.Today.AddDays(-todaysPosition+1);
            var upperDateRange = DateTime.Today.AddDays(NumberOfDaysToRender - todaysPosition);

            var meetings = await _meetingProvider.GetMeetings(lowerDateRange, upperDateRange);
            var dayViewModels = new List<DayViewModel>();

            for (var date = lowerDateRange; date <= upperDateRange; date = date.AddDays(1))
            {
                var meetingsForDate = meetings
                    .Where(x => x.ScheduledTime.Date == date.Date || x.EndTime.Date == date.Date)
                    .Select(x => _meetingViewModelFactory.Create(x))
                    .ToList();

                dayViewModels.Add(new DayViewModel(date, meetingsForDate, _viewModelRenderer, _modifyMeetingViewModelFactory));
            }

            return dayViewModels;
        }
    }
}
