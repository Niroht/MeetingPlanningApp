using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingPlanningApp.ViewModel;
using DataAccess.Common.Interfaces;
using MeetingPlanningApp.Services;
using DataAccess.Common.Model;

namespace MeetingPlanningApp.Factory
{
    public class ModifyMeetingViewModelFactory : IModifyMeetingViewModelFactory
    {
        private readonly IViewModelRenderer _viewModelRenderer;
        private readonly IMeetingPersister _meetingPersister;

        public ModifyMeetingViewModelFactory(IMeetingPersister meetingPersister, IViewModelRenderer viewModelRenderer)
        {
            _meetingPersister = meetingPersister;
            _viewModelRenderer = viewModelRenderer;
        }

        public ModifyMeetingViewModel Create()
        {
            return new ModifyMeetingViewModel(_meetingPersister, _viewModelRenderer);
        }

        public ModifyMeetingViewModel Create(Meeting meeting)
        {
            return new ModifyMeetingViewModel(meeting, _meetingPersister, _viewModelRenderer);
        }
    }
}
