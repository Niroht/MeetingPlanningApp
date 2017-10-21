using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Common.Model;
using MeetingPlanningApp.ViewModel;
using MeetingPlanningApp.Services;

namespace MeetingPlanningApp.Factory
{
    public class MeetingViewModelFactory : IMeetingViewModelFactory
    {
        private readonly IModifyMeetingViewModelFactory _modifyMeetingViewModelFactory;
        private readonly IViewModelRenderer _viewModelRenderer;

        public MeetingViewModelFactory(IViewModelRenderer viewModelRenderer, IModifyMeetingViewModelFactory modifyMeetingViewModelFactory)
        {
            _viewModelRenderer = viewModelRenderer;
            _modifyMeetingViewModelFactory = modifyMeetingViewModelFactory;
        }

        public MeetingViewModel Create(Meeting meeting)
        {
            return new MeetingViewModel(meeting, _viewModelRenderer, _modifyMeetingViewModelFactory);
        }
    }
}
