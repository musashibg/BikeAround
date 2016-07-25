using BikeAround.Service;
using GalaSoft.MvvmLight;
using System;
using System.Windows;
using System.Windows.Threading;

namespace BikeAround.App.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly DispatcherTimer _userUpdateTimer;

        private BikeAroundServiceClient _authenticatedClient;
        private PageViewModelBase _currentPage;
        private UserViewModel _currentUser;

        public PageViewModelBase CurrentPage
        {
            get { return _currentPage; }
            set
            {
                Set(nameof(CurrentPage), ref _currentPage, value);
            }
        }

        public UserViewModel CurrentUser
        {
            get { return _currentUser; }
            set
            {
                Set(nameof(CurrentUser), ref _currentUser, value);
            }
        }

        public MainViewModel()
        {
            _userUpdateTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle, Application.Current.Dispatcher)
            {
                Interval = TimeSpan.FromMinutes(1),
            };
            _userUpdateTimer.Tick += UserUpdateTimer_Tick;

            ShowLoginPage();
        }

        private void ShowLoginPage()
        {
            var loginPage = new LoginPageViewModel();
            loginPage.LoginSuccessful += LoginPage_LoginSuccessful;
            loginPage.RegisterTriggered += (s, e) => ShowRegisterUserPage();
            CurrentPage = loginPage;
        }

        private void ShowRegisterUserPage()
        {
            var registerUserPage = new RegisterUserPageViewModel();
            registerUserPage.BackToLoginTriggered += (s, e) => ShowLoginPage();
            CurrentPage = registerUserPage;
        }

        private void ShowMainMenuPage()
        {
            var mainMenuPage = new MainMenuPageViewModel();
            mainMenuPage.MyBikesTriggered += (s, e) => ShowMyBikesPage();
            mainMenuPage.FindABikeTriggered += (s, e) => ShowFindABikePage();
            mainMenuPage.MyTripsTriggered += (s, e) => ShowMyTripsPage();
            mainMenuPage.ExitApplicationTriggered += (s, e) => Application.Current.MainWindow.Close();
            CurrentPage = mainMenuPage;
        }

        private void ShowMyBikesPage()
        {
            var myBikesPage = new MyBikesPageViewModel(_authenticatedClient);
            myBikesPage.RegisterNewBikeTriggered += (s, e) => ShowRegisterBikePage();
            myBikesPage.ViewBikeTripsTriggered += (s, e) => ShowBikeTripsPage(e.BikeID);
            myBikesPage.BackToMenuTriggered += (s, e) => ShowMainMenuPage();
            CurrentPage = myBikesPage;
        }

        private void ShowFindABikePage()
        {
            var findABikePage = new FindABikePageViewModel(_authenticatedClient);
            findABikePage.BackToMenuTriggered += (s, e) => ShowMainMenuPage();
            CurrentPage = findABikePage;
        }

        private void ShowMyTripsPage()
        {
            var myTripsPage = new MyTripsPageViewModel(_authenticatedClient);
            myTripsPage.BackToMenuTriggered += (s, e) => ShowMainMenuPage();
            CurrentPage = myTripsPage;
        }

        private void ShowRegisterBikePage()
        {
            var registerBikePage = new RegisterBikePageViewModel(_authenticatedClient);
            registerBikePage.BackToListTriggered += (s, e) => ShowMyBikesPage();
            CurrentPage = registerBikePage;
        }

        private void ShowBikeTripsPage(int bikeID)
        {
            var bikeTripsPage = new BikeTripsPageViewModel(_authenticatedClient, bikeID);
            bikeTripsPage.BackToBikesListTriggered += (s, e) => ShowMyBikesPage();
            CurrentPage = bikeTripsPage;
        }

        private void UserUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Get up-to-date information about the current user and update the current user's view model
            User currentUser = _authenticatedClient.GetCurrentUser();
            CurrentUser.Update(currentUser);
        }

        private void LoginPage_LoginSuccessful(object sender, LoginSuccessfulEventArgs e)
        {
            _authenticatedClient = e.AuthenticatedClient;

            CurrentUser = new UserViewModel(_authenticatedClient.GetCurrentUser());
            _userUpdateTimer.IsEnabled = true;

            ShowMainMenuPage();
        }
    }
}