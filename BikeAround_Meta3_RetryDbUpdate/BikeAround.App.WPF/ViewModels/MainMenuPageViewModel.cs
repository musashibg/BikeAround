using System;
using System.Windows.Input;

namespace BikeAround.App.ViewModels
{
    public sealed class MainMenuPageViewModel : PageViewModelBase
    {
        public ICommand MyBikesCommand { get; }

        public ICommand FindABikeCommand { get; }

        public ICommand MyTripsCommand { get; }

        public ICommand ExitApplicationCommand { get; }

        public event EventHandler MyBikesTriggered;

        public event EventHandler FindABikeTriggered;

        public event EventHandler MyTripsTriggered;

        public event EventHandler ExitApplicationTriggered;

        public MainMenuPageViewModel()
        {
            MyBikesCommand = new RelayCommand(MyBikes);
            FindABikeCommand = new RelayCommand(FindABike);
            MyTripsCommand = new RelayCommand(MyTrips);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
        }

        private void MyBikes()
        {
            MyBikesTriggered?.Invoke(this, EventArgs.Empty);
        }

        private void FindABike()
        {
            FindABikeTriggered?.Invoke(this, EventArgs.Empty);
        }

        private void MyTrips()
        {
            MyTripsTriggered?.Invoke(this, EventArgs.Empty);
        }

        private void ExitApplication()
        {
            ExitApplicationTriggered?.Invoke(this, EventArgs.Empty);
        }
    }
}