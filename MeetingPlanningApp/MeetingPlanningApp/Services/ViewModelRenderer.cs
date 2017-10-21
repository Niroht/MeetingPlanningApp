using GalaSoft.MvvmLight;
using MeetingPlanningApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace MeetingPlanningApp.Services
{
    public class ViewModelRenderer : IViewModelRenderer
    {
        private readonly ViewModelLocator _locator;
        private readonly IUnityContainer _container;

        public ViewModelRenderer(ViewModelLocator locator, IUnityContainer container)
        {
            _locator = locator;
            _container = container;
        }

        public void HideModal()
        {
            _locator.Main.HideModalCommand.Execute(null);
        }

        public void RenderViewModel<T>() where T : ViewModelBase
        {
            _locator.Main.CurrentViewModel = _container.Resolve<T>();
        }

        public void RenderViewModelInModal(ViewModelBase viewModel)
        {
            _locator.Main.ModalViewModel = viewModel;
            _locator.Main.ModalVisibility = Visibility.Visible;
        }
    }
}
