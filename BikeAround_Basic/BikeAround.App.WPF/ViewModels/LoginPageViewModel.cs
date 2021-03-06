﻿using BikeAround.Service;
using System;
using System.Windows;
using System.Windows.Input;

namespace BikeAround.App.ViewModels
{
    public sealed class LoginPageViewModel : PageViewModelBase
    {
        private string _userName;
        private string _password;

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
                RaisePropertyChanged(nameof(CanLogin));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                {
                    return;
                }

                RaisePropertyChanging(nameof(Password));
                _password = value;
                RaisePropertyChanged(nameof(Password));
                RaisePropertyChanged(nameof(CanLogin));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrEmpty(Password);
            }
        }

        public ICommand LoginCommand { get; }

        public ICommand RegisterCommand { get; }

        public event EventHandler<LoginSuccessfulEventArgs> LoginSuccessful;

        public event EventHandler RegisterTriggered;

        public LoginPageViewModel()
        {
            LoginCommand = new RelayCommand(Login, () => CanLogin);
            RegisterCommand = new RelayCommand(Register);
        }

        private void Login()
        {
            var client = new BikeAroundServiceClient(UserName, Password);
            try
            {
                client.GetCurrentUser();
            }
            catch
            {
                MessageBox.Show("Failed to log in.", "Failure");
                return;
            }

            LoginSuccessful?.Invoke(this, new LoginSuccessfulEventArgs(client));
        }

        private void Register()
        {
            RegisterTriggered?.Invoke(this, EventArgs.Empty);
        }
    }
}