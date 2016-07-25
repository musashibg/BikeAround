using BikeAround.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BikeAround.App.ViewModels
{
    public sealed class RegisterBikePageViewModel : PageViewModelBase
    {
        private readonly BikeAroundServiceClient _authenticatedClient;

        private Guid? _bikeSecretIdentifier;

        public BikeViewModel Bike { get; }

        public ReadOnlyCollection<BikeKind> BikeKinds { get; }

        public ReadOnlyCollection<BrakeKind> BrakeKinds { get; }

        public Guid? BikeSecretIdentifier
        {
            get { return _bikeSecretIdentifier; }
            set
            {
                if (Set(nameof(BikeSecretIdentifier), ref _bikeSecretIdentifier, value))
                {
                    RaisePropertyChanged(nameof(CanRegister));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool CanRegister
        {
            get
            {
                return Bike.HourlyRate > 0m
                       && !string.IsNullOrEmpty(Bike.Make)
                       && !string.IsNullOrEmpty(Bike.Model)
                       && !string.IsNullOrEmpty(Bike.Color)
                       && Bike.LocationPostcode.HasValue
                       && !string.IsNullOrEmpty(Bike.LocationAddress)
                       && BikeSecretIdentifier.HasValue;
            }
        }

        public ICommand RegisterCommand { get; }

        public ICommand CancelCommand { get; }

        public event EventHandler BackToListTriggered;

        public RegisterBikePageViewModel(BikeAroundServiceClient authenticatedClient)
        {
            _authenticatedClient = authenticatedClient;
            Bike = new BikeViewModel();
            BikeKinds = new ReadOnlyCollection<BikeKind>((BikeKind[])Enum.GetValues(typeof(BikeKind)));
            BrakeKinds = new ReadOnlyCollection<BrakeKind>((BrakeKind[])Enum.GetValues(typeof(BrakeKind)));

            RegisterCommand = new RelayCommand(Register, () => CanRegister);
            CancelCommand = new RelayCommand(Cancel);

            Bike.PropertyChanged += Bike_PropertyChanged;
        }

        private void Register()
        {
            var bike = new Bike
            {
                HourlyRate = Bike.HourlyRate,
                Kind = Bike.Kind,
                Make = Bike.Make,
                Model = Bike.Model,
                Color = Bike.Color,
                Gears = Bike.Gears,
                Weight = Bike.Weight,
                FrontBrake = Bike.FrontBrake,
                BackBrake = Bike.BackBrake,
                Description = Bike.Description,
                LocationPostcode = Bike.LocationPostcode,
                LocationAddress = Bike.LocationAddress,
            };

            try
            {
                bike = _authenticatedClient.RegisterBike(bike, BikeSecretIdentifier.Value);
            }
            catch
            {
                MessageBox.Show("Registration failed.", "Failure");
                return;
            }

            BackToListTriggered?.Invoke(this, EventArgs.Empty);
        }

        private void Cancel()
        {
            BackToListTriggered?.Invoke(this, EventArgs.Empty);
        }

        private void Bike_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(CanRegister));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}