using MeetingPlanningApp.Services;
using MeetingPlanningApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace MeetingPlanningApp
{
    public class Bootstrapper
    {
        public static void RunApplication()
        {
            using (var container = new UnityContainer())
            {
                RegisterInterfaces(container);

                var renderer = container.Resolve<IViewModelRenderer>();
                renderer.RenderViewModel<ScheduleOverviewViewModel>();
            }
        }

        private static void RegisterInterfaces(UnityContainer container)
        {
            container.RegisterType<IViewModelRenderer, ViewModelRenderer>();
        }
    }
}
