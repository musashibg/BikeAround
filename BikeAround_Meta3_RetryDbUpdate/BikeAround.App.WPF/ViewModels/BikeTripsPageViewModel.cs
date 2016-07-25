using BikeAround.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BikeAround.App.ViewModels
{
    public sealed class BikeTripsPageViewModel : PageViewModelBase
    {
        private readonly BikeAroundServiceClient _authenticatedClient;
        private readonly int _bikeID;

        private TripViewModel _selectedTrip;

        public ObservableCollection<TripViewModel> Trips { get; }

        public TripViewModel SelectedTrip
        {
            get { return _selectedTrip; }
            set
            {
                if (Set(nameof(SelectedTrip), ref _selectedTrip, value))
                {
                    Application.Current.Dispatcher.InvokeAsync(() => value.LoadUser(_authenticatedClient));
                }
            }
        }

        public ICommand BackToBikesListCommand { get; }

        public event EventHandler BackToBikesListTriggered;

        public BikeTripsPageViewModel(BikeAroundServiceClient authenticatedClient, int bikeID)
        {
            _authenticatedClient = authenticatedClient;
            _bikeID = bikeID;
            Trips = new ObservableCollection<TripViewModel>();

            BackToBikesListCommand = new RelayCommand(BackToBikesList);

            LoadTripsList();
        }

        private void LoadTripsList()
        {
            Trips.Clear();
            try
            {
                Trip[] trips = _authenticatedClient.GetBikeTrips(_bikeID);
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

        private void BackToBikesList()
        {
            BackToBikesListTriggered?.Invoke(this, EventArgs.Empty);
        }
    }
}