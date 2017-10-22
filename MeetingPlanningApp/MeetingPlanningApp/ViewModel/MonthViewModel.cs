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
    public class MonthViewModel : ViewModelBase
    {
        public IEnumerable<DayViewModel> DayViewModels { get; }

        public string Month { get; }

        public MonthViewModel(
            IEnumerable<DayViewModel> dayViewModels, 
            string month)
        {
            Month = month;
            DayViewModels = dayViewModels;
        }
    }
}
