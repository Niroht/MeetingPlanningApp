using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace MeetingPlanningApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                Set(nameof(CurrentViewModel), ref _currentViewModel, value);
            }
        }

        private ViewModelBase _modalViewModel;
        public ViewModelBase ModalViewModel
        {
            get
            {
                return _modalViewModel;
            }
            set
            {
                Set(nameof(ModalViewModel), ref _modalViewModel, value);
            }
        }

        private Visibility _modalVisibility = Visibility.Collapsed;
        public Visibility ModalVisibility
        {
            get
            {
                return _modalVisibility;
            }
            set
            {
                Set(nameof(ModalVisibility), ref _modalVisibility, value);
            }
        }

        public ICommand HideModalCommand => new RelayCommand(() => ModalVisibility = Visibility.Collapsed);

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
        }
    }
}