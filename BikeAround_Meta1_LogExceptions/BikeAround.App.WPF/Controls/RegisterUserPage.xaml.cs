using BikeAround.App.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BikeAround.App.Controls
{
    /// <summary>
    /// Interaction logic for RegisterUserPage.xaml
    /// </summary>
    public partial class RegisterUserPage : UserControl
    {
        public RegisterUserPage()
        {
            InitializeComponent();
        }

        private void PasswordBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            ((RegisterUserPageViewModel)DataContext).Password = passwordBox.Password;
        }

        private void PasswordBoxPassword2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            ((RegisterUserPageViewModel)DataContext).Password2 = passwordBox.Password;
        }
    }
}