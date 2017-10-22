using DataAccess;
using DataAccess.Common.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using MeetingPlanningApp.Factory;
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
            container.RegisterType<IMeetingPersister, MeetingPersister>();
            container.RegisterType<IMeetingProvider, MeetingProvider>();
            container.RegisterType<IViewModelRenderer, ViewModelRenderer>();
            container.RegisterType<IMessenger, Messenger>(new ContainerControlledLifetimeManager());
            container.RegisterType<IModifyMeetingViewModelFactory, ModifyMeetingViewModelFactory>();
            container.RegisterType<IMeetingViewModelFactory, MeetingViewModelFactory>();
            container.RegisterType<IConflictFinder, ConflictFinder>();
            container.RegisterType<IMonthViewModelFactory, MonthViewModelFactory>();
        }
    }
}
