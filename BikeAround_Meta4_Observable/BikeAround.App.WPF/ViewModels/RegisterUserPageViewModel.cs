using BikeAround.App.Meta;
using BikeAround.Service;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BikeAround.App.ViewModels
{
    %Observable
    public sealed class RegisterUserPageViewModel : PageViewModelBase
    {
        private string _password;
        private string _password2;

        public UserViewModel User { get; }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(CanRegister));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Password2
        {
            get { return _password2; }
            set
            {
                _password2 = value;
                RaisePropertyChanged(nameof(CanRegister));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool CanRegister
        {
            get
            {
                return !string.IsNullOrWhiteSpace(User.UserName)
                       && !string.IsNullOrWhiteSpace(User.FullName)
                       && !string.IsNullOrWhiteSpace(User.Address)
                       && !string.IsNullOrWhiteSpace(User.Email)
                       && !string.IsNullOrEmpty(Password)
                       && Password == Password2;
            }
        }

        public ICommand RegisterCommand { get; }

        public ICommand CancelCommand { get; }

        public event EventHandler BackToLoginTriggered;

        public RegisterUserPageViewModel()
        {
            User = new UserViewModel();
            RegisterCommand = new RelayCommand(Register, () => CanRegister);
            CancelCommand = new RelayCommand(Cancel);

            User.PropertyChanged += User_PropertyChanged;
        }

        private void Register()
        {
            var user = new User
            {
                UserName = User.UserName,
                FullName = User.FullName,
                Address = User.Address,
                Email = User.Email,
            };

            var client = new BikeAroundServiceClient();
            try
            {
                user = client.RegisterUser(user, Password);
            }
            catch
            {
                MessageBox.Show("Registration failed.", "Failure");
                return;
            }

            BackToLoginTriggered?.Invoke(this, EventArgs.Empty);
        }

        private void Cancel()
        {
            BackToLoginTriggered?.Invoke(this, EventArgs.Empty);
        }

        private void User_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserViewModel.UserName)
                || e.PropertyName == nameof(UserViewModel.FullName)
                || e.PropertyName == nameof(UserViewModel.Address)
                || e.PropertyName == nameof(UserViewModel.Email))
            {
                RaisePropertyChanged(nameof(CanRegister));
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}