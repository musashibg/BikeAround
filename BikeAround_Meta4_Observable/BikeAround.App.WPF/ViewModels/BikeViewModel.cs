using BikeAround.App.Meta;
using BikeAround.Service;
using System.Windows;

namespace BikeAround.App.ViewModels
{
    %Observable
    public sealed class BikeViewModel
    {
        public int BikeID { get; }

        public int OwnerUserID { get; }

        public bool IsAvailable { get; }

        public decimal HourlyRate { get; set; }

        public BikeKind Kind { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public int? Gears { get; set; }

        public float? Weight { get; set; }

        public BrakeKind? FrontBrake { get; set; }

        public BrakeKind? BackBrake { get; set; }

        public string Description { get; set; }

        public int? LocationPostcode { get; set; }

        public string LocationAddress { get; set; }

        public UserViewModel OwnerUser { get; set; }

        public BikeViewModel()
        {
        }

        public BikeViewModel(Bike bike)
        {
            BikeID = bike.BikeID;
            OwnerUserID = bike.OwnerUserID;
            IsAvailable = bike.IsAvailable;
            HourlyRate = bike.HourlyRate;
            Kind = bike.Kind;
            Make = bike.Make;
            Model = bike.Model;
            Color = bike.Color;
            Gears = bike.Gears;
            Weight = bike.Weight;
            FrontBrake = bike.FrontBrake;
            BackBrake = bike.BackBrake;
            Description = bike.Description;
            LocationPostcode = bike.LocationPostcode;
            LocationAddress = bike.LocationAddress;
        }

        public void LoadOwnerUser(BikeAroundServiceClient authenticatedClient)
        {
            if (OwnerUser != null)
            {
                return;
            }

            try
            {
                User user = authenticatedClient.GetUser(OwnerUserID);
                OwnerUser = new UserViewModel(user);
            }
            catch
            {
                MessageBox.Show("Failed to load bike owner information.", "Failure");
            }
        }
    }
}