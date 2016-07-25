using BikeAround.Service;
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
                if (_userName == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(UserName));
                _userName = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(FullName));
                _fullName = value;
                RaisePropertyChanged(nameof(FullName));
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (_address == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Address));
                _address = value;
                RaisePropertyChanged(nameof(Address));
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Email));
                _email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }

        public int RentalOffers
        {
            get { return _rentalOffers; }
            set
            {
                if (_rentalOffers == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(RentalOffers));
                _rentalOffers = value;
                RaisePropertyChanged(nameof(RentalOffers));
            }
        }

        public int? TripCount
        {
            get { return _tripCount; }
            set
            {
                if (_tripCount == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(TripCount));
                _tripCount = value;
                RaisePropertyChanged(nameof(TripCount));
            }
        }

        public decimal? Balance
        {
            get { return _balance; }
            set
            {
                if (_balance == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Balance));
                _balance = value;
                RaisePropertyChanged(nameof(Balance));
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