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
        private readonly IConflictFinder _conflictFinder;

        public ModifyMeetingViewModelFactory(
            IMeetingPersister meetingPersister, 
            IViewModelRenderer viewModelRenderer,
            IConflictFinder conflictFinder)
        {
            _meetingPersister = meetingPersister;
            _viewModelRenderer = viewModelRenderer;
            _conflictFinder = conflictFinder;
        }

        public ModifyMeetingViewModel Create()
        {
            return new ModifyMeetingViewModel(_meetingPersister, _conflictFinder, _viewModelRenderer);
        }

        public ModifyMeetingViewModel Create(Meeting meeting)
        {
            return new ModifyMeetingViewModel(meeting, _meetingPersister, _conflictFinder, _viewModelRenderer);
        }
    }
}
