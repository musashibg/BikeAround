using BikeAround.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BikeAround.App.ViewModels
{
    public sealed class FindABikePageViewModel : PageViewModelBase
    {
        private readonly BikeAroundServiceClient _authenticatedClient;

        private int? _postcode;
        private BikeViewModel _selectedBike;

        public ObservableCollection<BikeViewModel> Bikes { get; }

        public int? Postcode
        {
            get { return _postcode; }
            set
            {
                if (_postcode == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Postcode));
                _postcode = value;
                RaisePropertyChanged(nameof(Postcode));
                RaisePropertyChanged(nameof(CanSearch));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public BikeViewModel SelectedBike
        {
            get { return _selectedBike; }
            set
            {
                if (_selectedBike == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(SelectedBike));
                _selectedBike = value;
                RaisePropertyChanged(nameof(SelectedBike));

                Application.Current.Dispatcher.InvokeAsync(() => value.LoadOwnerUser(_authenticatedClient));
            }
        }

        public bool CanSearch
        {
            get { return Postcode.HasValue; }
        }

        public ICommand SearchCommand { get; }

        public ICommand BackToMenuCommand { get; }

        public event EventHandler BackToMenuTriggered;

        public FindABikePageViewModel(BikeAroundServiceClient authenticatedClient)
        {
            _authenticatedClient = authenticatedClient;
            Bikes = new ObservableCollection<BikeViewModel>();

            SearchCommand = new RelayCommand(Search, () => CanSearch);
            BackToMenuCommand = new RelayCommand(BackToMenu);
        }

        private void Search()
        {
            Bikes.Clear();
            try
            {
                Bike[] bikes = _authenticatedClient.FindAvailableBikes(Postcode.Value);
                foreach (Bike bike in bikes)
                {
                    Bikes.Add(new BikeViewModel(bike));
                }
            }
            catch
            {
                MessageBox.Show("Failed to load bikes list.", "Failure");
            }
        }

        private void BackToMenu()
        {
            BackToMenuTriggered?.Invoke(this, EventArgs.Empty);
        }
    }
}