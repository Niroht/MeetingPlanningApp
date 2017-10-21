using DataAccess.Common.Model;
using MeetingPlanningApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingPlanningApp.Factory
{
    public interface IMeetingViewModelFactory
    {
        MeetingViewModel Create(Meeting meeting);
    }
}
