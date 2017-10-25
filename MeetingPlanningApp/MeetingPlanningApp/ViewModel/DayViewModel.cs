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
    public class DayViewModel : ViewModelBase
    {
        private readonly IModifyMeetingViewModelFactory _modifyMeetingViewModelFactory;
        private readonly IViewModelRenderer _viewModelRenderer;

        public ICommand NewMeetingCommand => new RelayCommand(OnCreateNewMeeting, () => CanCreateMeeting);

        public DateTime Date { get; }

        public bool IsCurrentMonth => Date.Month == DateTime.Now.Month;

        public bool CanCreateMeeting => Date.Date >= DateTime.Today;

        public IEnumerable<MeetingViewModel> Meetings { get; }

        public DayViewModel(
            DateTime date, 
            IEnumerable<MeetingViewModel> meetings,
            IViewModelRenderer viewModelRenderer,
            IModifyMeetingViewModelFactory modifyMeetingViewModelFactory)
        {
            _viewModelRenderer = viewModelRenderer;
            _modifyMeetingViewModelFactory = modifyMeetingViewModelFactory;

            Date = date;
            Meetings = meetings;
        }

        private void OnCreateNewMeeting()
        {
            var meeting = new Meeting(Date.Add(DateTime.Now.TimeOfDay), "", "", Enumerable.Empty<Attendee>(), TimeSpan.FromHours(1));
            _viewModelRenderer.RenderViewModelInModal(_modifyMeetingViewModelFactory.Create(meeting));
        }
    }
}
