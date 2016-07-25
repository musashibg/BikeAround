using BikeAround.App.Meta;
using BikeAround.Service;
using System;

namespace BikeAround.App.ViewModels
{
    %Observable
    public sealed class UserViewModel
    {
        public int UserID { get; }

        public Guid? SecretIdentifier { get; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public int RentalOffers { get; set; }

        public int? TripCount { get; set; }

        public decimal? Balance { get; set; }

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