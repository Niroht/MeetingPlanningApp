using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingPlanningApp.Services
{
    public interface IViewModelRenderer
    {
        void RenderViewModel<T>() where T : ViewModelBase;

        void RenderViewModelInModal(ViewModelBase viewModel);

        void HideModal();
    }
}
