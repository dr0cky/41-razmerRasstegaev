using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _41_razmerRasstegaev
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private async void LoginB_Click(object sender, RoutedEventArgs e)
        {
            string login = ULogin.Text;
            string password = UPassword.Text;
            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }

            User user = Rasstegaev41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);

            if (user != null)
            {
                Manager.MainFrame.Navigate(new BotinokPage(user));
                ULogin.Text = "";
                UPassword.Text = "";
            }
            else
            {
                MessageBox.Show("Введены неверные данные");
                ULogin.IsEnabled = false;
                await Task.Delay(10000);
                ULogin.IsEnabled = true;
            }
        }

        private void GuestB_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new BotinokPage(null));
            ULogin.Text = "";
            UPassword.Text = "";
        }
    }
}
