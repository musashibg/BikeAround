using BikeAround.App.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace BikeAround.App.WPF.Test
{
    internal sealed class ComplexPropertiesTestRun
    {
        private readonly Random _random;
        private readonly LoginPageViewModel _loginPage;

        public ComplexPropertiesTestRun(int seed)
        {
            _random = new Random(seed);
            _loginPage = new LoginPageViewModel();
            _loginPage.PropertyChanged += LoginPage_PropertyChanged;
        }

        public TimeSpan Perform(int count)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < count; i++)
            {
                switch (_random.Next(2))
                {
                    case 0:
                        TestSetUserName();
                        break;

                    case 1:
                        TestSetPassword();
                        break;
                }
            }
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }

        private void TestSetUserName()
        {
            string newUserName;
            if (_random.Next(2) == 0)
            {
                // 50% chance to retain the old value
                newUserName = _loginPage.UserName;
            }
            else
            {
                newUserName = TestData.GetRandomBikeMake(_random);
            }
            _loginPage.UserName = newUserName;
        }

        private void TestSetPassword()
        {
            string newPassword;
            if (_random.Next(2) == 0)
            {
                // 50% chance to retain the old value
                newPassword = _loginPage.Password;
            }
            else
            {
                newPassword = TestData.GetRandomBikeMake(_random);
            }
            _loginPage.Password = newPassword;
        }

        private void LoginPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
