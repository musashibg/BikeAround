using BikeAround.Service;
using GalaSoft.MvvmLight;
using System;

namespace BikeAround.App.ViewModels
{
    public sealed class UserViewModel : ViewModelBase
    {
        private string _userName;
        private string _fullName;
        private string _address;
        private string _email;
        private int _rentalOffers;
        private int? _tripCount;
        private decimal? _balance;

        public int UserID { get; }

        public Guid? SecretIdentifier { get; }

        public string UserName
        {
            get { return _userName; }
            set
            {
                Set(nameof(UserName), ref _userName, value);
            }
        }

        public string FullName
        {
            get { return _fullName; }
            set
            {
                Set(nameof(FullName), ref _fullName, value);
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                Set(nameof(Address), ref _address, value);
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                Set(nameof(Email), ref _email, value);
            }
        }

        public int RentalOffers
        {
            get { return _rentalOffers; }
            set
            {
                Set(nameof(RentalOffers), ref _rentalOffers, value);
            }
        }

        public int? TripCount
        {
            get { return _tripCount; }
            set
            {
                Set(nameof(TripCount), ref _tripCount, value);
            }
        }

        public decimal? Balance
        {
            get { return _balance; }
            set
            {
                Set(nameof(Balance), ref _balance, value);
            }
        }

        public UserViewModel()
        {
        }

        public UserViewModel(User user)
        {
            UserID = user.UserID;
            SecretIdentifier = user.SecretIdentifier;
            Update(user);
        }

        public void Update(User user)
        {
            UserName = user.UserName;
            FullName = user.FullName;
            Address = user.Address;
            Email = user.Email;
            RentalOffers = user.RentalOffers;
            TripCount = user.TripCount;
            Balance = user.Balance;
        }
    }
}