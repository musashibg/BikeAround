using BikeAround.Service;
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
                if (_hourlyRate == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(HourlyRate));
                _hourlyRate = value;
                RaisePropertyChanged(nameof(HourlyRate));
            }
        }

        public BikeKind Kind
        {
            get { return _kind; }
            set
            {
                if (_kind == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Kind));
                _kind = value;
                RaisePropertyChanged(nameof(Kind));
            }
        }

        public string Make
        {
            get { return _make; }
            set
            {
                if (_make == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Make));
                _make = value;
                RaisePropertyChanged(nameof(Make));
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                if (_model == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Model));
                _model = value;
                RaisePropertyChanged(nameof(Model));
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                if (_color == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Color));
                _color = value;
                RaisePropertyChanged(nameof(Color));
            }
        }

        public int? Gears
        {
            get { return _gears; }
            set
            {
                if (_gears == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Gears));
                _gears = value;
                RaisePropertyChanged(nameof(Gears));
            }
        }

        public float? Weight
        {
            get { return _weight; }
            set
            {
                if (_weight == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Weight));
                _weight = value;
                RaisePropertyChanged(nameof(Weight));
            }
        }

        public BrakeKind? FrontBrake
        {
            get { return _frontBrake; }
            set
            {
                if (_frontBrake == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(FrontBrake));
                _frontBrake = value;
                RaisePropertyChanged(nameof(FrontBrake));
            }
        }

        public BrakeKind? BackBrake
        {
            get { return _backBrake; }
            set
            {
                if (_backBrake == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(BackBrake));
                _backBrake = value;
                RaisePropertyChanged(nameof(BackBrake));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Description));
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public int? LocationPostcode
        {
            get { return _locationPostcode; }
            set
            {
                if (_locationPostcode == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(LocationPostcode));
                _locationPostcode = value;
                RaisePropertyChanged(nameof(LocationPostcode));
            }
        }

        public string LocationAddress
        {
            get { return _locationAddress; }
            set
            {
                if (_locationAddress == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(LocationAddress));
                _locationAddress = value;
                RaisePropertyChanged(nameof(LocationAddress));
            }
        }

        public UserViewModel OwnerUser
        {
            get { return _ownerUser; }
            set
            {
                if (_ownerUser == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(OwnerUser));
                _ownerUser = value;
                RaisePropertyChanged(nameof(OwnerUser));
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