﻿using BikeAround.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BikeAround.App.ViewModels
{
    public sealed class MyBikesPageViewModel : PageViewModelBase
    {
        private readonly BikeAroundServiceClient _authenticatedClient;

        private BikeViewModel _selectedBike;

        public ObservableCollection<BikeViewModel> Bikes { get; }

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
            }
        }

        public ICommand RegisterNewBikeCommand { get; }

        public ICommand ViewBikeTripsCommand { get; }

        public ICommand BackToMenuCommand { get; }

        public event EventHandler RegisterNewBikeTriggered;

        public event EventHandler<BikeDetailsEventArgs> ViewBikeTripsTriggered;

        public event EventHandler BackToMenuTriggered;

        public MyBikesPageViewModel(BikeAroundServiceClient authenticatedClient)
        {
            _authenticatedClient = authenticatedClient;
            Bikes = new ObservableCollection<BikeViewModel>();

            RegisterNewBikeCommand = new RelayCommand(RegisterNewBike);
            ViewBikeTripsCommand = new RelayCommand<int>(ViewBikeTrips);
            BackToMenuCommand = new RelayCommand(BackToMenu);

            LoadBikesList();
        }

        private void LoadBikesList()
        {
            Bikes.Clear();
            try
            {
                Bike[] bikes = _authenticatedClient.GetCurrentUserBikes();
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

        private void RegisterNewBike()
        {
            RegisterNewBikeTriggered?.Invoke(this, EventArgs.Empty);
        }

        private void ViewBikeTrips(int bikeID)
        {
            ViewBikeTripsTriggered?.Invoke(this, new BikeDetailsEventArgs(bikeID));
        }

        private void BackToMenu()
        {
            BackToMenuTriggered?.Invoke(this, EventArgs.Empty);
        }
    }
}