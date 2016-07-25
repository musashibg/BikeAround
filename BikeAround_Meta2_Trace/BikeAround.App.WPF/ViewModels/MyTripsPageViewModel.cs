using BikeAround.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BikeAround.App.ViewModels
{
    public sealed class MyTripsPageViewModel : PageViewModelBase
    {
        private readonly BikeAroundServiceClient _authenticatedClient;

        private TripViewModel _selectedTrip;

        public ObservableCollection<TripViewModel> Trips { get; }

        public TripViewModel SelectedTrip
        {
            get { return _selectedTrip; }
            set
            {
                if (Set(nameof(SelectedTrip), ref _selectedTrip, value))
                {
                    Application.Current.Dispatcher.InvokeAsync(() => value.LoadBike(_authenticatedClient));
                }
            }
        }

        public ICommand BackToMenuCommand { get; }

        public event EventHandler BackToMenuTriggered;

        public MyTripsPageViewModel(BikeAroundServiceClient authenticatedClient)
        {
            _authenticatedClient = authenticatedClient;
            Trips = new ObservableCollection<TripViewModel>();

            BackToMenuCommand = new RelayCommand(BackToMenu);

            LoadTripsList();
        }

        private void LoadTripsList()
        {
            Trips.Clear();
            try
            {
                Trip[] trips = _authenticatedClient.GetCurrentUserTrips();
                foreach (Trip trip in trips)
                {
                    Trips.Add(new TripViewModel(trip));
                }
            }
            catch
            {
                MessageBox.Show("Failed to load trips list.", "Failure");
            }
        }

        private void BackToMenu()
        {
            BackToMenuTriggered?.Invoke(this, EventArgs.Empty);
        }
    }
}