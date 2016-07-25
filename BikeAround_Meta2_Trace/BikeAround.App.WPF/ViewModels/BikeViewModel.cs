using BikeAround.Service;
using GalaSoft.MvvmLight;
using System.Windows;

namespace BikeAround.App.ViewModels
{
    public sealed class BikeViewModel : ViewModelBase
    {
        private decimal _hourlyRate;
        private BikeKind _kind;
        private string _make;
        private string _model;
        private string _color;
        private int? _gears;
        private float? _weight;
        private BrakeKind? _frontBrake;
        private BrakeKind? _backBrake;
        private string _description;
        private int? _locationPostcode;
        private string _locationAddress;
        private UserViewModel _ownerUser;

        public int BikeID { get; }

        public int OwnerUserID { get; }

        public bool IsAvailable { get; }

        public decimal HourlyRate
        {
            get { return _hourlyRate; }
            set
            {
                Set(nameof(HourlyRate), ref _hourlyRate, value);
            }
        }

        public BikeKind Kind
        {
            get { return _kind; }
            set
            {
                Set(nameof(Kind), ref _kind, value);
            }
        }

        public string Make
        {
            get { return _make; }
            set
            {
                Set(nameof(Make), ref _make, value);
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                Set(nameof(Model), ref _model, value);
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                Set(nameof(Color), ref _color, value);
            }
        }

        public int? Gears
        {
            get { return _gears; }
            set
            {
                Set(nameof(Gears), ref _gears, value);
            }
        }

        public float? Weight
        {
            get { return _weight; }
            set
            {
                Set(nameof(Weight), ref _weight, value);
            }
        }

        public BrakeKind? FrontBrake
        {
            get { return _frontBrake; }
            set
            {
                Set(nameof(FrontBrake), ref _frontBrake, value);
            }
        }

        public BrakeKind? BackBrake
        {
            get { return _backBrake; }
            set
            {
                Set(nameof(BackBrake), ref _backBrake, value);
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                Set(nameof(Description), ref _description, value);
            }
        }

        public int? LocationPostcode
        {
            get { return _locationPostcode; }
            set
            {
                Set(nameof(LocationPostcode), ref _locationPostcode, value);
            }
        }

        public string LocationAddress
        {
            get { return _locationAddress; }
            set
            {
                Set(nameof(LocationAddress), ref _locationAddress, value);
            }
        }

        public UserViewModel OwnerUser
        {
            get { return _ownerUser; }
            set
            {
                Set(nameof(OwnerUser), ref _ownerUser, value);
            }
        }

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