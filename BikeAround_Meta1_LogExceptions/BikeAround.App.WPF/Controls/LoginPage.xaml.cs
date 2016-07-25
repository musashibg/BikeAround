using BikeAround.App.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BikeAround.App.Controls
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            ((LoginPageViewModel)DataContext).Password = passwordBox.Password;
        }
    }
}