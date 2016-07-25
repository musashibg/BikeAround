using BikeAround.Service;
using System;
using System.Windows;

namespace BikeAround.App.ViewModels
{
    public sealed class TripViewModel : ViewModelBase
    {
        private UserViewModel _user;
        private BikeViewModel _bike;

        public int TripID { get; }

        public int UserID { get; }

        public int BikeID { get; }

        public DateTime TripStart { get; }

        public DateTime TripEnd { get; }

        public decimal TripCost { get; }

        public decimal ServiceFee { get; }

        public UserViewModel User
        {
            get { return _user; }
            set
            {
                if (_user == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(User));
                _user = value;
                RaisePropertyChanged(nameof(User));
            }
        }

        public BikeViewModel Bike
        {
            get { return _bike; }
            set
            {
                if (_bike == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Bike));
                _bike = value;
                RaisePropertyChanged(nameof(Bike));
            }
        }

        public TripViewModel(Trip trip)
        {
            TripID = trip.TripID;
            UserID = trip.UserID;
            BikeID = trip.BikeID;
            TripStart = trip.TripStart;
            TripEnd = trip.TripEnd;
            TripCost = trip.TripCost;
            ServiceFee = trip.ServiceFee;
        }

        public void LoadUser(BikeAroundServiceClient authenticatedClient)
        {
            if (User != null)
            {
                return;
            }

            try
            {
                User user = authenticatedClient.GetUser(UserID);
                User = new UserViewModel(user);
            }
            catch
            {
                MessageBox.Show("Failed to load trip user information.", "Failure");
            }
        }

        public void LoadBike(BikeAroundServiceClient authenticatedClient)
        {
            if (Bike != null)
            {
                return;
            }

            try
            {
                Bike bike = authenticatedClient.GetBike(BikeID);
                Bike = new BikeViewModel(bike);
            }
            catch
            {
                MessageBox.Show("Failed to load trip bike information.", "Failure");
            }
        }
    }
}