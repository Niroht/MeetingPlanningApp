using DataAccess;
using DataAccess.Common.Interfaces;
using MeetingPlanningApp.Services;
using MeetingPlanningApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

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
            container.RegisterType<IMeetingStore, MeetingStore>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMeetingPersister, MeetingPersister>();
            container.RegisterType<IMeetingProvider, MeetingProvider>();
            container.RegisterType<IViewModelRenderer, ViewModelRenderer>();
        }
    }
}
